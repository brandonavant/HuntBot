using System;
using HuntBot.Infrastructure.Database.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HuntBot.Infrastructure
{
    public static class InfrastructureDependencies
    {
        private static SqliteConnectionFactory _sqliteConnectionFactory = SqliteConnectionFactory.GetInstance();    
        
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            InitializeDatabase();

            services.AddSingleton<SqliteConnectionFactory>(_sqliteConnectionFactory);
            return services;
        }

        /// <summary>
        /// Creates the database along with its tables.
        /// </summary>
        private static void InitializeDatabase()
        {
            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var command = connection.CreateCommand();

                command.CommandText = "CREATE TABLE IF NOT EXISTS HuntBotGames (ID INT, TEXT Aggregate)";
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Log.Logger.Fatal(ex, "Unable to initialize database.");
                throw;
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }
        }
    }

}
