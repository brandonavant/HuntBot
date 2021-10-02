using System.Collections.Concurrent;

namespace HuntBot.Domain.HuntBotGames.GameState
{
    /// <summary>
    /// Thread-safe dictionary used to pass bot state information between threads.
    /// </summary>
    public class BotStateLookup : ConcurrentDictionary<string, object> { }
}
