using System.Threading.Tasks;
using Api.Battleships.Database.Models;
using Api.Battleships.Services;
using Api.Battleships.Services.Models;
using Api.Battleships.Tests.Helpers;
using Api.Battleships.v1.Controllers;
using Api.Battleships.v1.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Api.Battleships.Tests.v1.Controllers
{
	[TestFixture]
	public class BattleshipsControllerTests
	{
		private Mock<IGameService> _mockGameService;

		private BattleshipsController _battleshipsController;

		[SetUp]
		public void SetUp()
		{
			_mockGameService = new Mock<IGameService>(MockBehavior.Strict);

			_battleshipsController = new BattleshipsController(
				_mockGameService.Object,
				Mock.Of<ILogger<BattleshipsController>>());
		}

		[Test]
		public async Task PostNewGameAsync_ReturnsExpectedNewGameResponse()
		{
			// Arrange
			_mockGameService
				.Setup(m => m.CreateGameAsync(2, 8, 20))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PostNewGameAsync();

			// Assert
			var actualNewGameResponse = actualActionResult.AssertOkGetValue<NewGameResponse>();

			Assert.AreEqual(1, actualNewGameResponse.GameId);
			Assert.AreEqual(8, actualNewGameResponse.BoardSize);
			Assert.AreEqual(20, actualNewGameResponse.GuessesRemaining);
			Assert.AreEqual(2, actualNewGameResponse.ShipsRemaining);

			_mockGameService.Verify();
		}

		[Test]
		public async Task PatchFireTorpedoAsync_WithValidCoordinates_ReturnsExpectedFireTorpedoResponse()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = 5,
			};

			_mockGameService
				.Setup(m => m.GetGameAsync(1))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			_mockGameService
				.Setup(m => m.TorpedoAlreadyFiredAsync(1,
					It.Is<Coordinate>(c => c.Row == 3 && c.Column == 5)))
				.ReturnsAsync(false)
				.Verifiable();

			_mockGameService
				.Setup(m => m.FireTorpedoAsync(1,
					It.Is<Coordinate>(c => c.Row == 3 && c.Column == 5)))
				.ReturnsAsync(new TorpedoResult
				{
					Distance = 0,
					ShipSunk = true,
					GuessesRemaining = 5,
					ShipsRemaining = 3,
				})
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(1, request);

			// Assert
			var actualFireTorpedoResponse = actualActionResult.AssertOkGetValue<FireTorpedoResponse>();

			Assert.AreEqual(5, actualFireTorpedoResponse.GuessesRemaining);
			Assert.AreEqual(3, actualFireTorpedoResponse.ShipsRemaining);
			Assert.AreEqual(0, actualFireTorpedoResponse.Distance);
			Assert.AreEqual(true, actualFireTorpedoResponse.ShipSunk);

			_mockGameService.Verify();
		}

		[Test]
		public async Task PatchFireTorpedoAsync_WithInvalidGameId_ReturnsExpectedBadRequest()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = 5,
			};

			_mockGameService
				.Setup(m => m.GetGameAsync(0))
				.ReturnsAsync((Game)null);

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(0, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Must specify an existing game to fire a torpedo", badRequestMessage);
		}

		[Test]
		public async Task PatchFireTorpedoAsync_WithMissingRequestBody_ReturnsExpectedBadRequest()
		{
			// Arrange
			_mockGameService
				.Setup(m => m.GetGameAsync(1))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(1, null);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Must specify row and column in request body", badRequestMessage);

			_mockGameService.Verify();
		}

		[Test]
		[TestCase(0)]
		[TestCase(9)]
		public async Task PatchFireTorpedoAsync_WithInvalidRow_ReturnsExpectedBadRequest(int row)
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = row,
				Column = 5,
			};

			_mockGameService
				.Setup(m => m.GetGameAsync(1))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(1, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Row must be in the range 1 - 8 (inclusive)", badRequestMessage);

			_mockGameService.Verify();
		}

		[Test]
		[TestCase(0)]
		[TestCase(9)]
		public async Task PatchFireTorpedoAsync_WithInvalidColumn_ReturnsExpectedBadRequest(int column)
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = column,
			};

			
			_mockGameService
				.Setup(m => m.GetGameAsync(1))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(1, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Column must be in the range 1 - 8 (inclusive)", badRequestMessage);

			_mockGameService.Verify();
		}

		[Test]
		public async Task PatchFireTorpedoAsync_WithAlreadyFiredCoordinates_ReturnsExpectedBadRequest()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = 5,
			};

			_mockGameService
				.Setup(m => m.GetGameAsync(1))
				.ReturnsAsync(new Game
				{
					Id = 1,
					BoardSize = 8,
					TotalGuesses = 20,
					TotalShips = 2,
				})
				.Verifiable();

			_mockGameService
				.Setup(m => m.TorpedoAlreadyFiredAsync(1,
					It.Is<Coordinate>(c => c.Row == 3 && c.Column == 5)))
				.ReturnsAsync(true)
				.Verifiable();

			// Act
			var actualActionResult = await _battleshipsController.PatchFireTorpedoAsync(1, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Already fired there, try somewhere new", badRequestMessage);

			_mockGameService.Verify();
		}
	}
}
