namespace Api.Battleships.v1.Models
{
	public class NewGameResponse
	{
		/// <summary>
		/// The number of guesses available to sink all the ships.
		/// </summary>
		public int Guesses { get; set; }
		/// <summary>
		/// The number of ships to sink.
		/// </summary>x
		public int Ships { get; set; }
	}
}
