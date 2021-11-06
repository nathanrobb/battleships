using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Api.Battleships.Database
{
	public class InMemoryDbConnection : IDisposable
	{
		private readonly object _connectionLock = new object();

		private DbConnection _connection;

		public DbConnection DbConnection => _connection ?? CreateInMemoryDatabase();

		private DbConnection CreateInMemoryDatabase()
		{
			lock (_connectionLock)
			{
				if (_connection != null)
					return _connection;

				// Note: Disposing this connection removes the database from memory.
				_connection = new SqliteConnection("Data Source=:memory:");
				_connection.Open();

				return _connection;
			}
		}

		public void Dispose()
		{
			lock (_connectionLock)
			{
				_connection?.Dispose();
				_connection = null;
			}
		}
	}
}
