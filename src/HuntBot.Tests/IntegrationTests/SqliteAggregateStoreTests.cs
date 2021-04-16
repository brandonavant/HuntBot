using System;
using System.Collections.Generic;
using HuntBot.Domain.HuntBotGames;
using HuntBot.Infrastructure.Database.Sqlite;
using HuntBot.Infrastructure.EventStore;
using Xunit;

namespace HuntBot.Tests.IntegrationTests
{
    public class SqliteAggregateStoreTests
    {
        [Fact]
        public async void SqliteAggregateStore_Save_SerializesDataAsByteArrayCorrectly()
        {
            var huntBotGameId = new Guid("C0A4DBC6-C4AB-4AD9-894C-01FBF37CF4E8");

            HuntBotGame huntBotGame = HuntBotGame.CreateNewHuntBotGame(
                huntBotGameId, 
                "Easter Huntbot Game", 
                DateTime.UtcNow.AddDays(1), 
                DateTime.UtcNow.AddDays(7), 
                new List<string>()
            );

            huntBotGame.AddGameObject(1000, "Droog", 5002);

            SqliteConnectionFactory sqliteConnectionFactory = SqliteConnectionFactory.GetInstance();
            SqliteAggregateStore store = new SqliteAggregateStore(sqliteConnectionFactory);
            
            await store.Save<HuntBotGame>(huntBotGame);
        }
    }
}