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

			Assert.AreEqual(1, actualNewGameResponse.GameId);
			Assert.AreEqual(8, actualNewGameResponse.BoardSize);
			Assert.AreEqual(20, actualNewGameResponse.GuessesRemaining);
			Assert.AreEqual(2, actualNewGameResponse.ShipsRemaining);
		}

		[Test]
		public void PatchFireTorpedo_WithValidCoordinates_ReturnsExpectedFireTorpedoResponse()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = 5,
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(1, request);

			// Assert
			var actualFireTorpedoResponse = actualActionResult.AssertOkGetValue<FireTorpedoResponse>();

			Assert.AreEqual(20, actualFireTorpedoResponse.GuessesRemaining);
			Assert.AreEqual(2, actualFireTorpedoResponse.ShipsRemaining);
			Assert.AreEqual(4, actualFireTorpedoResponse.Distance);
			Assert.AreEqual(false, actualFireTorpedoResponse.ShipSunk);
		}

		[Test]
		public void PatchFireTorpedo_WithInvalidGameId_ReturnsExpectedBadRequest()
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = 5,
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(0, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Must specify an existing game to fire a torpedo", badRequestMessage);
		}

		[Test]
		public void PatchFireTorpedo_WithMissingRequestBody_ReturnsExpectedBadRequest()
		{
			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(1, null);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Must specify row and column in request body", badRequestMessage);
		}

		[Test]
		[TestCase(0)]
		[TestCase(9)]
		public void PatchFireTorpedo_WithInvalidRow_ReturnsExpectedBadRequest(int row)
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = row,
				Column = 5,
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(1, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Row must be in the range 1 - 8 (inclusive)", badRequestMessage);
		}

		[Test]
		[TestCase(0)]
		[TestCase(9)]
		public void PatchFireTorpedo_WithInvalidColumn_ReturnsExpectedBadRequest(int column)
		{
			// Arrange
			var request = new FireTorpedoRequest
			{
				Row = 3,
				Column = column,
			};

			// Act
			var actualActionResult = _battleshipsController.PatchFireTorpedo(1, request);

			// Assert
			var badRequestMessage = actualActionResult.AssertBadRequestGetMessage();

			Assert.AreEqual("Column must be in the range 1 - 8 (inclusive)", badRequestMessage);
		}
	}
}
