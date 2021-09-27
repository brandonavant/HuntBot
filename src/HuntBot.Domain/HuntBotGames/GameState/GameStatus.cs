namespace HuntBot.Domain.HuntBotGames.GameState
{
    /// <summary>
    /// Available HuntBot game statuses.
    /// </summary>
    public enum GameStatus
    {
        PendingLogin,
        LoggingIn,
        QueryingEnvironment,
        Running
    }
}
