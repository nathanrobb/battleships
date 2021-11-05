namespace Api.Battleships.v1.Models
{
	public class FireTorpedoResponse
	{
		/// <summary>
		/// The number of guesses remaining to sink all the ships.
		/// </summary>
		/// <remarks>When this reaches 0 (and at least 1 ship remains), the game is lost.</remarks>
		public int GuessesRemaining { get; set; }
		/// <summary>
		/// The number of ships remaining to sink.
		/// </summary>
		/// <remarks>When this reaches 0, the game is won.</remarks>
		public int ShipsRemaining { get; set; }
		/// <summary>
		/// The proximity of the fired torpedo, one of either: hit, hot, warm, cold.
		/// </summary>
		/// <remarks>
		/// Hit is when a ship cell is hit.
		/// Hot is when a ship cell is within 1-2 cells.
		/// Warm is when a ship cell is within 3-4 cells.
		/// Cold is when a ship cell is greater than 4 cells away.
		/// </remarks>
		// TODO: return an enum/number instead?
		public string ShipProximity { get; set; }
		/// <summary>
		/// When true, the previously fired torpedo hit and sunk a ship.
		/// </summary>
		public bool ShipSunk { get; set; }
	}
}
