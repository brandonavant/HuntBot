using System;
using System.Threading.Tasks;
using HuntBot.Domain.SeedWork;
using HuntBot.Infrastructure.Database.Sqlite;

namespace HuntBot.Infrastructure.EventStore
{
    public class SqliteAggregateStore : IAggregateStore
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public SqliteAggregateStore(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public Task<bool> Exists<T>(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task<T> Load<T>(Guid aggregateId) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public Task Save<T>(T aggregate) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}