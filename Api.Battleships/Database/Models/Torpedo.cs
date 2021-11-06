using Microsoft.EntityFrameworkCore;

namespace Api.Battleships.Database.Models
{
	public class Torpedo
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }

		public virtual Game Game { get; set; }
		public virtual ShipCell HitShip { get; set; }
	}

	internal static class TorpedoMapping
	{
		internal static ModelBuilder MapTorpedo(this ModelBuilder modelBuilder)
		{
			return modelBuilder.Entity<Torpedo>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("torpedo");

				entity.HasIndex(e => new
				{
					e.GameId,
					e.Row,
					e.Column,
				}).IsUnique();

				entity
					.Property(e => e.Id)
					.HasColumnName("id");

				entity.HasIndex(e => e.GameId);

				entity
					.Property(e => e.GameId)
					.HasColumnName("gameid");

				entity
					.Property(e => e.Row)
					.HasColumnName("row");

				entity
					.Property(e => e.Column)
					.HasColumnName("column");

				entity
					.HasOne(s => s.Game)
					.WithMany(g => g.Torpedoes)
					.HasForeignKey(s => s.GameId)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});
		}
	}
}
