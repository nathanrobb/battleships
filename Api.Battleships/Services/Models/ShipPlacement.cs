using System.Collections.Generic;

namespace Api.Battleships.Services.Models
{
	public class ShipPlacement
	{
		public bool IsHorizontal { get; set;  }
		/// <summary>
		/// A collection of coordinates representing a ship on the battleships board.
		/// </summary>
		public ICollection<Coordinate> Coordinates { get; set; }
	}
}
