using System;
using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using HuntBot.Domain.SeedWork;
using HuntBot.Infrastructure.Database.Sqlite;
using HuntBot.Infrastructure.EventStore;
using HuntBot.Infrastructure.Repositories;
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

            services.AddSingleton(SqliteConnectionFactory.GetInstance());
            services.AddSingleton<IAggregateStore>(new SqliteAggregateStore(_sqliteConnectionFactory));
            services.AddTransient<IHuntBotConfigRepository, HuntBotConfigRepository>();

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

                command.CommandText = SqlStatements.CreateHuntBotGamesTableIfNotExists;
                command.ExecuteNonQuery();

                command.CommandText = SqlStatements.CreateJsonStoreTableIfNotExists;
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
