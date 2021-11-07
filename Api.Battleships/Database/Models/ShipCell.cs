using Microsoft.EntityFrameworkCore;

namespace Api.Battleships.Database.Models
{
	public class ShipCell
	{
		public int Id { get; set; }
		public int ShipId { get; set; }
		public int? TorpedoId { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }

		public virtual Ship Ship { get; set; }
		public virtual Torpedo HitByTorpedo { get; set; }
	}

	internal static class ShipCellMapping
	{
		internal static ModelBuilder MapShipCell(this ModelBuilder modelBuilder)
		{
			return modelBuilder.Entity<ShipCell>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("shipcell");

				entity
					.Property(e => e.Id)
					.HasColumnName("id");

				entity.HasIndex(e => e.ShipId);
				entity.HasIndex(e => e.TorpedoId);

				entity
					.Property(e => e.ShipId)
					.HasColumnName("shipid");

				entity
					.Property(e => e.TorpedoId)
					.HasColumnName("torpedoid")
					.IsRequired(false);

				entity
					.Property(e => e.Row)
					.HasColumnName("row");

				entity
					.Property(e => e.Column)
					.HasColumnName("column");

				entity
					.HasOne(c => c.Ship)
					.WithMany(s => s.ShipCells)
					.HasForeignKey(c => c.ShipId)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();

				entity
					.HasOne(c => c.HitByTorpedo)
					.WithOne(t => t.HitShip)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(false);
			});
		}
	}
}
