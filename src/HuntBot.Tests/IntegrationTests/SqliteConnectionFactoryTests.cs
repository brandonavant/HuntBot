using System;
using System.IO;
using System.Threading.Tasks;
using HuntBot.Infrastructure.Database.Sqlite;
using Xunit;

namespace HuntBot.Tests.IntegrationTests
{
    public class SqliteConnectionFactoryTests
    {
        private SqliteConnectionFactory _sqliteConnectionFactory;

        public SqliteConnectionFactoryTests()
        {
            _sqliteConnectionFactory = SqliteConnectionFactory.GetInstance();
        }

        // public void Dispose()
        // {
        //     if (File.Exists("huntbot.db"))
        //     {
        //         File.Delete("huntbot.db");
        //     }
        // }

        [Fact]
        public void SqliteConnectionFactory_GetConnection_ProvidesReadAndWriteAccessToDatabase()
        {
            int numberOfRecords = 0;
            int id = 0;

            try
            {
                var sqliteConnection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var command = sqliteConnection.CreateCommand();

                command.CommandText = "CREATE TABLE Test1 (Id INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Test1 (Id) VALUES (1000)";
                command.ExecuteNonQuery();
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }

            try
            {
                var sqliteConnection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Read);
                var command = sqliteConnection.CreateCommand();

                command.CommandText = "SELECT Id FROM Test1;";
                
                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        id = reader.GetInt32(0);

                        numberOfRecords++;
                    }
                }
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
                
            Assert.Equal(1, numberOfRecords);
            Assert.Equal(1000, id);
        }

        [Fact]
        public void SqliteConnectionFactory_MultipleWrites_AllWritesAreGivenLockEventually()
        {
            Task firstTask = Task.Factory.StartNew(() => TaskOneMethod());
            Task secondTask = Task.Factory.StartNew(() => TaskTwoMethod());

            try
            {
                var sqliteConnection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var command = sqliteConnection.CreateCommand();

                command.CommandText = "CREATE TABLE Test2 (Id INT)";
                command.ExecuteNonQuery();
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }

            Task.Delay(100);

            Task.WaitAll(firstTask, secondTask);
        }

        private void TaskOneMethod()
        {
            try
            {
                var sqliteConnection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var command = sqliteConnection.CreateCommand();

                command.CommandText = "INSERT INTO Test2 (Id) VALUES (1000)";
                command.ExecuteNonQuery();
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }

        private void TaskTwoMethod()
        {
            try
            {
                var sqliteConnection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var command = sqliteConnection.CreateCommand();

                command.CommandText = "INSERT INTO Test2 (Id) VALUES (1001)";
                command.ExecuteNonQuery();                
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }
    }
}