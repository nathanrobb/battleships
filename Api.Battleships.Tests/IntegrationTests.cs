using System.Threading.Tasks;
using Api.Battleships.Database;
using Api.Battleships.Services;
using Api.Battleships.Tests.Helpers;
using Api.Battleships.v1.Controllers;
using Api.Battleships.v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Api.Battleships.Tests
{
	/// <summary>
	/// Tests the DI container brings and whole API to database pipeline.
	/// </summary>
	[TestFixture]
	public class IntegrationTests
	{
		private Mock<IRandomGenerator> _mockRandomGenerator;

		private ServiceProvider _serviceProvider;

		private BattleshipsController _battleshipsController;

		[SetUp]
		public void SetUp()
		{
			_mockRandomGenerator = new Mock<IRandomGenerator>();
			var prevRandom = 0;
			_mockRandomGenerator
				.Setup(m => m.GetRandomIntBetween(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(() => prevRandom++);

			var serviceCollection = new ServiceCollection();

			var mockConfiguration = new Mock<IConfiguration>();

			// Configure our DI container.
			new Startup(mockConfiguration.Object)
				.ConfigureServices(serviceCollection);

			// Replace our random with a predictable mock.
			serviceCollection.Replace(ServiceDescriptor.Scoped(s => _mockRandomGenerator.Object));

			_serviceProvider = serviceCollection.BuildServiceProvider();

			_battleshipsController = new BattleshipsController(
				_serviceProvider.GetRequiredService<IGameService>(),
				_serviceProvider.GetRequiredService<ILogger<BattleshipsController>>());
		}

		[TearDown]
		public async Task TearDownAsync()
		{
			if (_serviceProvider != null)
				await _serviceProvider.DisposeAsync();
		}

		[Test]
		public async Task PostNewGameAsync()
		{
			// Act
			var actualActionResult = await _battleshipsController.PostNewGameAsync();

			// Assert
			var actualNewGameResponse = actualActionResult.AssertOkGetValue<NewGameResponse>();

			var assertContext = _serviceProvider.GetRequiredService<BattleshipsContext>();
			var actualGame = await assertContext.Games.FindAsync(actualNewGameResponse.GameId);

			Assert.IsNotNull(actualGame);
		}

		[Test]
		public async Task PatchFireTorpedoAsync()
		{
			// Arrange
			// Create a game to fire a torpedo on
			var newGameActionResult = await _battleshipsController.PostNewGameAsync();
			var newGameResponse = newGameActionResult.AssertOkGetValue<NewGameResponse>();

			var gameId = newGameResponse.GameId;

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(gameId, new FireTorpedoRequest
			{
				Row = 1,
				Column = 2,
			});

			// Assert
			var actualFireTorpedoResponse = actualActionResult.AssertOkGetValue<FireTorpedoResponse>();

			Assert.AreEqual(19, actualFireTorpedoResponse.GuessesRemaining);
			Assert.AreEqual(2, actualFireTorpedoResponse.ShipsRemaining);
			Assert.AreEqual(0, actualFireTorpedoResponse.Distance);
			Assert.AreEqual(false, actualFireTorpedoResponse.ShipSunk);

			var assertContext = _serviceProvider.GetRequiredService<BattleshipsContext>();
			var actualTorpedo = await assertContext.Torpedoes
				.Include(t => t.HitShip)
				.SingleOrDefaultAsync(t => t.GameId == gameId);

			Assert.IsNotNull(actualTorpedo);
			
			Assert.AreEqual(1, actualTorpedo.Row);
			Assert.AreEqual(2, actualTorpedo.Column);

			Assert.AreEqual(1, actualTorpedo.HitShip.Row);
			Assert.AreEqual(2, actualTorpedo.HitShip.Column);
		}
	}
}
