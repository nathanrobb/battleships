namespace Api.Battleships.v1.Models
{
	/// <summary>
	/// The PATCH /v1/battleships/{gameId}/fire-torpedo request object.
	/// </summary>
	public class FireTorpedoRequest
	{
		/// <summary>
		/// The row coordinate to fire the torpedo at.
		/// </summary>
		public int Row { get; set; }
		/// <summary>
		/// The column coordinate to fire the torpedo at.
		/// </summary>
		public int Column { get; set; }
	}
}
