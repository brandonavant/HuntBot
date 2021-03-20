namespace HuntBot.Domain.HuntBotGame
{
    public interface IGameUniquenessChecker
    {
        /// <summary>
        /// Determines if the given game title is unique.
        /// </summary>
        /// <param name="title">The game title whose character length is being checked.</param>
        /// <returns>True if the game title is not already in use.</returns>
        bool IsUnique(string title);
    }
}
