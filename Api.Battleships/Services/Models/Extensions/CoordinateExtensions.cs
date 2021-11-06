using System;

// Note: Same namespace as Coordinate for nicer intellisense.
namespace Api.Battleships.Services.Models
{
	public static class CoordinateExtensions
	{
		/// <summary>
		/// Calculate the manhattan distance between two points.
		/// </summary>
		/// <remarks>This is the sum of the absolute difference between two coordinate points.
		/// See https://en.wikipedia.org/wiki/Taxicab_geometry for more info.</remarks>
		public static int GetManhattanDistance(this Coordinate from, Coordinate to)
		{
			if (from == null)
				throw new ArgumentNullException(nameof(from));

			if (to == null)
				throw new ArgumentNullException(nameof(to));

			return Math.Abs(from.Row - to.Row) + Math.Abs(from.Column - to.Column);
		}
	}
}
