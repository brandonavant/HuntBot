using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.SeedWork;
using HuntBot.Infrastructure.Database.Sqlite;
using Serilog;

namespace HuntBot.Infrastructure.EventStore
{
    public class SqliteAggregateStore : IAggregateStore
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public SqliteAggregateStore(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task<bool> Exists<T>(Guid aggregateId)
        {
            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);
                var parameters = new { Id = aggregateId };
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM HuntBotGames WHERE Id = @Id", parameters);

                return count == 1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to lookup aggregate {aggregateId}.", aggregateId);
            }

            return false;
        }

        public async Task<T> Load<T>(Guid aggregateId) where T : AggregateRoot
        {
            

            try
            {
                var aggregate = (T)Activator.CreateInstance(typeof(T), true);
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);
                var parameters = new { Id = aggregateId };
                var aggregateRecord = await connection.QueryAsync<HuntBotGame>("SELECT Id FROM HuntBotGames WHERE Id = @Id", parameters);

                
                
                // TODO: Load the entire aggregate record from the store
                // TODO: Get collection of changes in Changes column
                // TODO: Iterate through each change and Load onto aggregate
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to lookup aggregate {aggregateId}.", aggregateId);
            }

            throw new NotImplementedException();
        }

        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}