using Dapper;
using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using HuntBot.Infrastructure.Database.Sqlite;
using HuntBot.Infrastructure.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Infrastructure.Repositories
{
    public class HuntBotConfigRepository : IHuntBotConfigRepository
    {
        /// <summary>
        /// Key with which the single HuntBotConfig record is retrieved from the JsonStore table.
        /// </summary>
        private const string HuntBotConfigKey = "HuntBotConfig";

        /// <summary>
        /// SQLite connection factory with which connectivity is established.
        /// </summary>
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntBotConfigRepository"/> class.
        /// </summary>
        /// <param name="sqliteConnectionFactory"></param>
        public HuntBotConfigRepository(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        /// <summary>
        /// Loads the <see cref="HuntBotConfig"/> instance.
        /// </summary>
        /// <returns>The loaded <see cref="HuntBotConfig"/> instance.</returns>
        public async Task<HuntBotConfig> LoadSettings()
        {
            HuntBotConfig huntBotConfig = null;

            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);
                var jsonStoreObject = await connection.QueryFirstOrDefaultAsync<JsonStoreObject>(
                    SqlStatements.GetHuntBotConfig, 
                    new { 
                        Key = HuntBotConfigKey 
                    }
                );

                if (jsonStoreObject is not null)
                {
                    huntBotConfig = JsonConvert.DeserializeObject<HuntBotConfig>(
                        Encoding.UTF8.GetString(jsonStoreObject.Value)
                    );
                }

                return huntBotConfig;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to load HuntBotConfig.");
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }

            return null;
        }

        /// <summary>
        /// Saves the given <see cref="HuntBotConfig"/> instance.
        /// </summary>
        /// <param name="settings">The <see cref="HuntBotConfig"/> instance to be saved.</param>
        /// <returns>True if the save was successful; false otherwise.</returns>
        public async Task<bool> SaveSettings(HuntBotConfig settings)
        {
            try
            {
                var connection = _sqliteConnectionFactory.GetConnection(SqliteConnectionMode.Write);

                await connection.ExecuteAsync(SqlStatements.UpsertHuntBotConfig,
                    new
                    {
                        Key = HuntBotConfigKey,
                        Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(settings))
                    }
                );

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to save HuntBotConfig");
            }
            finally
            {
                _sqliteConnectionFactory.ReleaseConnection();
            }

            return false;
        }
    }
}
