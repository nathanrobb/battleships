using Api.Battleships.Database.Models;

namespace Api.Battleships.Services.Models
{
	public class TorpedoDistance
	{
		/// <summary>
		/// The ship cell that the distance was calculated from.
		/// </summary>
		public ShipCell ShipCell { get; }
		/// <summary>
		/// The distance the torpedo was from the ship cell.
		/// </summary>
		public int Distance { get; }

		public TorpedoDistance(ShipCell shipCell, int distance)
		{
			ShipCell = shipCell;
			Distance = distance;
		}
	}
}
