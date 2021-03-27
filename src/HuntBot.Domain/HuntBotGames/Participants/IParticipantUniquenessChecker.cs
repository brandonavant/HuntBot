using System;

namespace HuntBot.Domain.HuntBotGames.Participants
{
    public interface IParticipantUniquenessChecker
    {
        /// <summary>
        /// Determines if the given citizen number has already been registered as a participant in this game.
        /// </summary>
        /// <param name="citizenNumber">The citizen number of the participant to look up.</param>
        /// <param name="huntBotGameId">The id of the <see cref="HuntBotGame"/> for which a participant's registration is checked.</param>
        /// <returns>True if the participant has not already been added to the game.</returns>
        bool IsUnique(Guid huntBotGameId, int citizenNumber);
    }
}
