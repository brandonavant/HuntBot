namespace HuntBot.Infrastructure.Database.Sqlite
{
    /// <summary>
    /// Enumeration that specifies the type of connection that a client wishes to open.
    /// </summary>
    public enum SqliteConnectionMode
    {
        Read = 1,
        Write = 2,
        NotLocked = 3
    }
}