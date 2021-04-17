using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
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
                var existingAggregateRecord = await connection.QueryFirstOrDefaultAsync<StoredAggregate>(
                    Queries.GetAggregateById, 
                    new 
                    { 
                        AggregateId = aggregateId
                        .ToString()
                        .ToUpper() 
                    }
                );

                if (existingAggregateRecord is null)
                {
                    return null;
                }

                var events = JsonConvert.DeserializeObject<List<StoredEvent>>(
                    Encoding.UTF8.GetString(existingAggregateRecord.StoredEvents)
                );

                // TODO bavant: Need to figure out how to implement IAsyncEnumerable
                var history = events.Select(@event =>
                {
                    var clrType = Type.GetType(@event.ClrType);
                    var jsonData = Encoding.UTF8.GetString(@event.Data.ToArray());
                    
                    return JsonConvert.DeserializeObject(jsonData, clrType);
                });

                aggregate.Load(history);

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
            List<StoredEvent> eventsToStore = new List<StoredEvent>();

            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var nextVersion = 0;
                var existingAggregateRecord = await connection.QueryFirstOrDefaultAsync<StoredAggregate>(
                    Queries.GetAggregateById, 
                    new 
                    { 
                        AggregateId = aggregate
                        .Id
                        .ToString()
                        .ToUpper() 
                    }
                );
                
                if (existingAggregateRecord is not null)
                {
                    eventsToStore = JsonConvert.DeserializeObject<List<StoredEvent>>(
                        Encoding.UTF8.GetString(existingAggregateRecord.StoredEvents)
                    );

                    nextVersion = eventsToStore.Max(ets => ets.StreamPosition) + 1;
                }

                var changesToAdd = aggregate.GetChanges().ToList();

                if (!changesToAdd.Any())
                {
                    return;
                }

                foreach(var @event in changesToAdd)
                {
                    eventsToStore.Add(new StoredEvent(
                        nextVersion,
                        Serialize(@event),
                        @event.GetType().AssemblyQualifiedName
                    ));

                    nextVersion++;
                }
                var parameters = new { Id = aggregate.Id, StoredEvents = Serialize(eventsToStore) };

                await connection.ExecuteAsync(Queries.InsertNewHuntBotGame, parameters);

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