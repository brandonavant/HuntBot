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
        /// <summary>
        /// SQLite connection factory with which connectivity is established.
        /// </summary>
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;
        
        /// <summary>
        /// Initializes a new instance of <see cref="SqliteAggregateStore"/>.
        /// </summary>
        /// <param name="sqliteConnectionFactory">SQLite connection factory with which connectivity is established.</param>
        public SqliteAggregateStore(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }
        
        /// <summary>
        /// Checks to see if an aggregate exists in the store.
        /// </summary>
        /// <param name="aggregateId">The Id of the aggregate.see cref=""/></param>
        /// <typeparam name="T">The CLR type of the aggregate.</typeparam>
        /// <returns>True if the aggregate is found in the store.</returns>
        public async Task<bool> Exists<T>(Guid aggregateId)
        {
            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);
                var count = await connection.ExecuteScalarAsync<int>(SqlStatements.CheckIfAggregateExists, new { Id = aggregateId });
                
                if (count > 1)
                {
                    Log.Logger.Error("Found duplicate records for Aggregate with Id @aggregateId", aggregateId);
                }

                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to lookup aggregate {aggregateId}.", aggregateId);
            }

            return false;
        }

        /// <summary>
        /// Loads an aggregate record from the aggregate store.
        /// </summary>
        /// <param name="aggregateId">The Id of the aggregate being loaded.</param>
        /// <typeparam name="T">The CLR type of the aggregate.</typeparam>
        /// <returns>True if the aggregate is found in the store.</returns>
        public async Task<T> Load<T>(Guid aggregateId) where T : AggregateRoot
        {
            try
            {
                var aggregate = (T)Activator.CreateInstance(typeof(T), true);
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);
                var existingAggregateRecord = await connection.QuerySingleOrDefaultAsync<StoredAggregate>(
                    SqlStatements.GetAggregateById, 
                    new 
                    { 
                        AggregateId = aggregateId
                        .ToString()
                        .ToUpper() 
                    }
                );

                if (existingAggregateRecord is not null)
                {
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
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to lookup aggregate {aggregateId}.", aggregateId);
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }

            return null;
        }

        /// <summary>
        /// Loads a collection of all aggregates in the aggregate store.
        /// </summary>
        /// <typeparam name="T">The CLR type of the aggregate.</typeparam>
        /// <returns>A list of aggregates.</returns>
        public async Task<List<T>> LoadAll<T>() where T : AggregateRoot
        {
            try
            {
                List<T> aggregates = new List<T>();

                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);        
                var aggregateIds = await connection.QueryAsync<string>(SqlStatements.GetAggregateIds);
                
                // Must release the connection for the Load method to be able to use it.
                _sqliteConnectionFactory.ReleaseConnection();

                foreach (var id in aggregateIds)
                {
                    aggregates.Add(await this.Load<T>(new Guid(id)));
                }

                return aggregates;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to load list of aggregate ids");
                throw;
            }
        }

        /// <summary>
        /// Stores a set of changes for a given aggregate record.
        /// </summary>
        /// <param name="aggregate">The aggregate record whose changes are being stored.</param>
        /// <typeparam name="T">The CLR type of the aggregate.</typeparam>
        public async Task Save<T>(T aggregate) where T : AggregateRoot
        {
            List<StoredEvent> eventsToStore = new List<StoredEvent>();

            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var nextVersion = 0;
                var existingAggregateRecord = await connection.QuerySingleOrDefaultAsync<StoredAggregate>(
                    SqlStatements.GetAggregateById, 
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

                await connection.ExecuteAsync(SqlStatements.UpsertNewHuntBotGame, parameters);

                aggregate.ClearChanges();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to load aggregate {aggregateId}.", aggregate.Id);
                throw;
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }

        /// <summary>
        /// Serializes an object to raw bytes.
        /// </summary>
        /// <param name="data">To data that is being serialized.</param>
        /// <returns>The raw bytes of the serialized object.</returns>
        public static byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}