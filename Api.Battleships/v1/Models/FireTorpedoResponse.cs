namespace Api.Battleships.v1.Models
{
	/// <summary>
	/// The PATCH /v1/battleships/{gameId}/fire-torpedo response object.
	/// </summary>
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
		/// The manhattan distance of the fired torpedo to the nearest ship.
		/// </summary>
		/// <remarks>
		public int Distance { get; set; }
		/// <summary>
		/// When true, the previously fired torpedo hit and sunk a ship.
		/// </summary>
		public bool ShipSunk { get; set; }
	}
}
