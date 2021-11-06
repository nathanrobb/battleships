using System.Collections.Generic;
using Api.Battleships.Database.Models;
using Api.Battleships.Services.Models;

namespace Api.Battleships.Services
{
	public class ShipDistanceService
	{
		/// <summary>
		/// Get the minimum manhattan distance between the torpedo and the un-hit ship cells.
		/// </summary>
		/// <param name="torpedo">The fired torpedo coordinate.</param>
		/// <param name="shipCells">The un-hit ship cells to find the distance to.</param>
		/// <returns>An object with the closest ship cell and the distance the torpedo was from that cell.</returns>
		public TorpedoDistance GetClosestShipCell(Coordinate torpedo, IEnumerable<ShipCell> shipCells)
		{
			TorpedoDistance minTorpedoDistance = null;
			foreach (var cell in shipCells)
			{
				var shipCellCoordinate = new Coordinate(cell.Row, cell.Column);

				var distance = torpedo.GetManhattanDistance(shipCellCoordinate);
				if (distance == 0)
					return new TorpedoDistance(cell, 0);

				if (minTorpedoDistance == null || minTorpedoDistance.Distance > distance)
					minTorpedoDistance = new TorpedoDistance(cell, distance);
			}

			return minTorpedoDistance;
		}
	}
}
