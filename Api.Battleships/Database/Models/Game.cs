using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Battleships.Database.Models
{
	public class Game
	{
		public int Id { get; set; }
		public int BoardSize { get; set; }
		public int TotalGuesses { get; set; }
		public int TotalShips { get; set; }

		public virtual ICollection<Ship> Ships { get; set; } = new HashSet<Ship>();
		public virtual ICollection<Torpedo> Torpedoes { get; set; } = new HashSet<Torpedo>();
	}

	internal static class GameMapping
	{
		internal static ModelBuilder MapGame(this ModelBuilder modelBuilder)
		{
			return modelBuilder.Entity<Game>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("game");

				entity.Property(e => e.Id)
					.HasColumnName("id");

				entity.Property(e => e.BoardSize)
					.HasColumnName("boardsize");

				entity.Property(e => e.TotalGuesses)
					.HasColumnName("totalguesses");

				entity.Property(e => e.TotalShips)
					.HasColumnName("totalships");
			});
		}
	}
}
