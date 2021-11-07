using System.Linq;
using Api.Battleships.Services;
using Moq;
using NUnit.Framework;

namespace Api.Battleships.Tests.Services
{
	[TestFixture]
	public class ShipPlacerServiceTests
	{
		private Mock<IRandomGenerator> _mockRandomGenerator;

		private ShipPlacerService _shipPlacerService;

		[SetUp]
		public void SetUp()
		{
			_mockRandomGenerator = new Mock<IRandomGenerator>(MockBehavior.Strict);

			_shipPlacerService = new ShipPlacerService(_mockRandomGenerator.Object);
		}

		[Test]
		public void GetShipPlacements_KnownPlacement_ReturnsExpectedPlacements()
		{
			// Arrange
			_mockRandomGenerator
				.Setup(m => m.GetRandomBool())
				.Returns(false);

			var prevRand = 0;
			_mockRandomGenerator
				.Setup(m => m.GetRandomIntBetween(0, 56))
				.Returns(() => prevRand++);

			// Act
			var actual = _shipPlacerService.GetShipPlacements(2, 8);

			// Assert
			var actualPlacements = actual.ToArray();
			var actualShipCells = actualPlacements.SelectMany(p => p.Coordinates).ToArray();

			Assert.AreEqual(2, actualPlacements.Length);
			Assert.AreEqual(4, actualShipCells.Length);

			//vertical ship, (1,1), (2,1) (top left corner, column 1)
			Assert.IsFalse(actualPlacements[0].IsHorizontal);
			Assert.AreEqual(1, actualShipCells[0].Row);
			Assert.AreEqual(1, actualShipCells[0].Column);
			Assert.AreEqual(2, actualShipCells[1].Row);
			Assert.AreEqual(1, actualShipCells[1].Column);

			// vertical ship, (1,2), (2,2) (top left corner, column 2)
			Assert.IsFalse(actualPlacements[1].IsHorizontal);
			Assert.AreEqual(1, actualShipCells[2].Row);
			Assert.AreEqual(2, actualShipCells[2].Column);
			Assert.AreEqual(2, actualShipCells[3].Row);
			Assert.AreEqual(2, actualShipCells[3].Column);
		}
	}
}
