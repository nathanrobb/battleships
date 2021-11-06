using Api.Battleships.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Battleships.Database
{
	public class BattleshipsContext : DbContext
	{
		public virtual DbSet<Game> Games { get; set; }
		public virtual DbSet<Ship> Ships { get; set; }
		public virtual DbSet<ShipCell> ShipCells { get; set; }
		public virtual DbSet<Torpedo> Torpedoes { get; set; }

		public BattleshipsContext(DbContextOptions<BattleshipsContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.MapGame()
				.MapShip()
				.MapShipCell()
				.MapTorpedo();
		}
	}
}
