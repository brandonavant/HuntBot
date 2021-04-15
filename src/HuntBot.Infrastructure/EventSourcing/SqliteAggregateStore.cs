using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.SeedWork;
using HuntBot.Infrastructure.Database.Sqlite;
using HuntBot.Infrastructure.Models;
using Newtonsoft.Json;
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
                var storedAggregate = await connection.QuerySingleOrDefaultAsync<StoredAggregate>("SELECT Id FROM HuntBotGames WHERE Id = @Id", parameters);

                // TODO: Load the entire aggregate record from the store
                // TODO: Get collection of changes in Changes column
                // TODO: Iterate through each change and Load onto aggregate

                return aggregate;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to lookup aggregate {aggregateId}.", aggregateId);
                return null;
            }
            finally
            {   
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }

        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            var changes = aggregate
                .GetChanges()
                .Select(@event =>
                    new StoredEvent(
                        aggregate.Version,
                        Serialize(@event),
                        @event.GetType().AssemblyQualifiedName
                    )
                );
            
            if (!changes.Any())
            {
                return;
            }

            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var parameters = new { Id = aggregate.Id, StoredEvents = Serialize(changes) };

                // TODO: Below is just a quick test. I actually need to load anything that exists already and append this set of changes.
                await connection.ExecuteAsync("INSERT INTO HuntBotGames (Id, StoredEvents) VALUES(@Id, @StoredEvents) ON CONFLICT(Id) DO UPDATE SET StoredEvents=@StoredEvents", parameters);

                aggregate.ClearChanges();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to load aggregate {aggregateId}.", aggregate.Id);
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }

        /// <summary>
        /// Serializes an object to raw bytes.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The raw bytes of the serialized object.</returns>
        public static byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}