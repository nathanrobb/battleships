namespace Api.Battleships.Services.Models
{
	public class TorpedoResult
	{
		/// <summary>
		/// The proximity of the fired torpedo, one of either: hit, hot, warm, cold.
		/// </summary>
		public int Distance { get; set; }
		/// <summary>
		/// When true, the previously fired torpedo hit and sunk a ship.
		/// </summary>
		public bool ShipSunk { get; set; }
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
	}
}
