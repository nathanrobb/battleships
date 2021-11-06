using Api.Battleships.Database.Models;
using Api.Battleships.Services;
using Api.Battleships.Services.Models;
using NUnit.Framework;

namespace Api.Battleships.Tests.Services
{
	[TestFixture]
	public class ShipDistanceServiceTests
	{
		private ShipDistanceService _shipDistanceService;

		[SetUp]
		public void SetUp()
		{
			 _shipDistanceService = new ShipDistanceService();
		}

		[Test]
		public void GetClosestShipCell_MultipleCells_ReturnsClosesCell()
		{
			// Arrange
			var torpedo = new Coordinate(3, 5);
			var shipCells = new []
			{
				new ShipCell { Row = 2, Column = 7 },
				new ShipCell { Row = 2, Column = 8 },
			};

			// Act
			var actual = _shipDistanceService.GetClosestShipCell(torpedo, shipCells);

			// Assert
			Assert.IsNotNull(actual);
			Assert.AreEqual(shipCells[0], actual.ShipCell);
			Assert.AreEqual(3, actual.Distance);
		}

		[Test]
		public void GetClosestShipCell_NoCells_ReturnsNull()
		{
			// Arrange
			var torpedo = new Coordinate(1, 1);
			var shipCells = new ShipCell[0];

			// Act
			var actual = _shipDistanceService.GetClosestShipCell(torpedo, shipCells);

			// Assert
			Assert.IsNull(actual);
		}
	}
}
