using Api.Battleships.Tests.Helpers;
using Api.Battleships.v1.Controllers;
using Api.Battleships.v1.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Api.Battleships.Tests.v1.Controllers
{
	public class BattleshipsControllerTests
	{
		private BattleshipsController _battleshipsController;

		[SetUp]
		public void SetUp()
		{
			_battleshipsController = new BattleshipsController(Mock.Of<ILogger<BattleshipsController>>());
		}

		[Test]
		public void PostNewGame_ReturnsExpectedNewGameResponse()
		{
			// Act
			var actualActionResult = _battleshipsController.PostNewGame();

			// Assert
			var actualNewGameResponse = actualActionResult.AssertOkGetValue<NewGameResponse>();

			Assert.AreEqual(20, actualNewGameResponse.Guesses);
			Assert.AreEqual(2, actualNewGameResponse.Ships);
		}

		[Test]
		public void PatchFireTorpedo_WithValidCoordinates_ReturnsExpectedFireTorpedoResponse()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Coordinate = "3,7",
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(request);

			// Assert
			var actualFireTorpedoResponse = actualActionResult.AssertOkGetValue<FireTorpedoResponse>();

			Assert.AreEqual(20, actualFireTorpedoResponse.GuessesRemaining);
			Assert.AreEqual(2, actualFireTorpedoResponse.ShipsRemaining);
			Assert.AreEqual("", actualFireTorpedoResponse.ShipProximity);
			Assert.AreEqual(false, actualFireTorpedoResponse.ShipSunk);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("   ")]
		public void PatchFireTorpedo_WithNullOrWhitespaceCoordinates_ReturnsExpectBadRequest(string coordinates)
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Coordinate = coordinates,
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Must specify coordinates in request body", badRequestMessage);
		}
	}
}
