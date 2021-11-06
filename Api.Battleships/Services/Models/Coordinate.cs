namespace Api.Battleships.Services.Models
{
	/// <summary>
	/// A coordinate on the battleships grid.
	/// </summary>
	public class Coordinate
	{
		/// <summary>
		/// Vertical coordinate.
		/// </summary>
		public int Row { get; }
		/// <summary>
		/// Horizontal coordinate.
		/// </summary>
		public int Column { get; }

		public Coordinate(int row, int column)
		{
			Row = row;
			Column = column;
		}
	}
}
