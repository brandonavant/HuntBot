namespace HuntBot.Infrastructure.Database.Sqlite
{
    /// <summary>
    /// SQL Command Constants
    /// </summary>
    public static class SqlStatements
    {
        /// <summary>
        /// Gets a record from the aggregate store by its Id.
        /// </summary>
        public const string GetAggregateById = "SELECT Id, StoredEvents FROM HuntBotGames WHERE Id = @AggregateId";

        /// <summary>
        /// Inserts a new HuntBotGame record into the aggregate store.
        /// </summary>
        public const string InsertNewHuntBotGame = "INSERT INTO HuntBotGames (Id, StoredEvents) VALUES(@Id, @StoredEvents) ON CONFLICT(Id) DO UPDATE SET StoredEvents=@StoredEvents";

        /// <summary>
        /// Returns the count of aggregate records with a given aggregate Id. There should only ever be one, so this should be used to check if a record exists.
        /// </summary>
        public const string CheckIfAggregateExists = "SELECT COUNT(*) FROM HuntBotGames WHERE Id = @Id";

        /// <summary>
        /// Creates the HuntBotGames table in the database.
        /// </summary>
        public const string CreateHuntBotGamesTableIfNotExists = "CREATE TABLE IF NOT EXISTS HuntBotGames (Id TEXT NOT NULL PRIMARY KEY, StoredEvents BLOB)";

        /// <summary>
        /// Gets a collection of all of the aggregate ids.
        /// </summary>
        public const string GetAggregateIds = "SELECT Id FROM HuntBotGames";
    }
}