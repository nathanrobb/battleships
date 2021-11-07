using System;
using System.Collections.Generic;
using System.Linq;
using Api.Battleships.Services.Models;

namespace Api.Battleships.Services
{
	public class ShipPlacerService
	{
		private readonly IRandomGenerator _random;

		public ShipPlacerService(IRandomGenerator random)
		{
			_random = random;
		}

		/// <summary>
		/// Get the placements on the board for <see cref="shipCount"/> ships.
		/// </summary>
		/// <param name="shipCount">The number of ships to place on the board.</param>
		/// <param name="boardSize">The size of the board, must be at least 2.</param>
		/// <returns></returns>
		public IEnumerable<ShipPlacement> GetShipPlacements(int shipCount, int boardSize)
		{
			// Make sure our board can actually place a ship.
			if (shipCount == 0 || boardSize < 1)
				yield break;

			var placedCoordinates = new List<Coordinate>();
			for (var i = 0; i < shipCount; i++)
			{
				var placement = GetDistinctPlacement(boardSize, placedCoordinates);
				placedCoordinates.AddRange(placement.Coordinates);
				yield return placement;
			}
		}

		private ShipPlacement GetDistinctPlacement(int boardSize, ICollection<Coordinate> placedCoordinates)
		{
			// Once a ship is on the grid, we need to avoid overlapping ships. For now just loop until no overlap...
			// 2 ships on a 64 cell grid should not loop frequently.
			// However, if the ship count increases or the board size decreases we may hit performance issues.

			// TODO: make this algorithm smarter with deterministic looping.

			ShipPlacement placement;
			var loopCount = 0;
			do
			{
				
				placement = GetPlacement(boardSize);
				loopCount++;
			}
			while (
				// Just in case we get stuck in a loop, exit after 10 tries.
				loopCount < 10 &&
				// Retry if any new placement cells clash with an existing ship.
				placedCoordinates.Any(p => placement.Coordinates.Any(n => n.Row == p.Row && n.Column == p.Column))
			);

			if (loopCount >= 10)
				throw new Exception("Unable to place ships distinctly on the board, try with different ship count / board size.");

			return placement;
		}

		private ShipPlacement GetPlacement(int boardSize)
		{
			// Our ship is a 1x2 (horizontal) or a 2x1 (vertical).
			// We can place the left cell of a horizontal ship anywhere on a 8 by 7 grid.
			// We can place the top cell of a vertical ship anywhere on a 7 by 8 grid.

			// TODO: make this algorithm be other ship aware to avoid already occupied cells.

			var availableCells = boardSize * (boardSize - 1);
			var cell = _random.GetRandomIntBetween(0, availableCells);
			var isHorizontal = _random.GetRandomBool();

			Coordinate shipCoordinate1;
			Coordinate shipCoordinate2;

			// 8 by 7
			if (isHorizontal)
			{
				// + 1 to be 1-base index
				var row = (cell % boardSize) + 1;
				// + 1 to be 1-base index
				var col = (cell / boardSize) + 1;
				
				shipCoordinate1 = new Coordinate(row, col);
				shipCoordinate2 = new Coordinate(row, col + 1);
			}
			// 7 by 8
			else
			{

				// + 1 to be 1-base index
				var row = (cell / boardSize) + 1;
				// + 1 to be 1-base index
				var col = (cell % boardSize) + 1;

				shipCoordinate1 = new Coordinate(row, col);
				shipCoordinate2 = new Coordinate(row + 1, col);
			}

			return new ShipPlacement
			{
				IsHorizontal = isHorizontal,
				Coordinates = new [] {shipCoordinate1, shipCoordinate2}
			};
		}
	}
}
