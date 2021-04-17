namespace HuntBot.Infrastructure.Database.Sqlite
{
    public static class Queries
    {
        public const string GetAggregateById = "SELECT Id, StoredEvents FROM HuntBotGames WHERE Id = @AggregateId";
        public const string InsertNewHuntBotGame = "INSERT INTO HuntBotGames (Id, StoredEvents) VALUES(@Id, @StoredEvents) ON CONFLICT(Id) DO UPDATE SET StoredEvents=@StoredEvents";
        public const string CheckIfAggregateExists = "SELECT COUNT(*) FROM HuntBotGames WHERE Id = @Id";
    }
}