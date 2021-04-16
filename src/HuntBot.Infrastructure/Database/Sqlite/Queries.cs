namespace HuntBot.Infrastructure.Database.Sqlite
{
    public static class Queries
    {
        public const string GetAggregateById = "SELECT Id, StoredEvents FROM HuntBotGames WHERE Id = @AggregateId";
    }
}