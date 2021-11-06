using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Battleships.Database.Models
{
	public class Ship
	{
		public int Id { get; set; }
		public int GameId { get; set; }

		public virtual Game Game { get; set; }
		public virtual ICollection<ShipCell> ShipCells { get; set; } = new HashSet<ShipCell>();
	}

	internal static class ShipMapping
	{
		internal static ModelBuilder MapShip(this ModelBuilder modelBuilder)
		{
			return modelBuilder.Entity<Ship>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("ship");

				entity
					.Property(e => e.Id)
					.HasColumnName("id");

				entity.HasIndex(e => e.GameId);

				entity
					.Property(e => e.GameId)
					.HasColumnName("gameid");

				entity
					.HasOne(s => s.Game)
					.WithMany(g => g.Ships)
					.HasForeignKey(s => s.GameId)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
		}
	}
}
