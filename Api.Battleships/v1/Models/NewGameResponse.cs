namespace Api.Battleships.v1.Models
{
	/// <summary>
	/// The POST /v1/battleships/new-game response object.
	/// </summary>
	public class NewGameResponse
	{
		/// <summary>
		/// The database id for the new battleships game.
		/// </summary>
		public int GameId { get; set; }
		/// <summary>
		/// The number rows and columns where the ships can be placed.
		/// </summary>
		public int BoardSize { get; set; }
		/// <summary>
		/// The number of guesses remaining to sink all the ships.
		/// </summary>
		public int GuessesRemaining { get; set; }
		/// <summary>
		/// The number of ships remaining to sink.
		/// </summary>
		public int ShipsRemaining { get; set; }
	}
}
