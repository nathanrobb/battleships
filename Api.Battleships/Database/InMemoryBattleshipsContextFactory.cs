using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Battleships.Database
{
	public class InMemoryBattleshipsContextFactory : IDbContextFactory<BattleshipsContext>
	{
		private readonly InMemoryDbConnection _connection;
		private readonly ILoggerFactory _loggerFactory;

		public InMemoryBattleshipsContextFactory(InMemoryDbConnection connection, ILoggerFactory loggerFactory)
		{
			_connection = connection;
			_loggerFactory = loggerFactory;
		}

		public BattleshipsContext CreateDbContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<BattleshipsContext>();
			optionsBuilder
				.UseSqlite(_connection.DbConnection)
				.UseLoggerFactory(_loggerFactory);

			return new BattleshipsContext(optionsBuilder.Options);
		}
	}
}
