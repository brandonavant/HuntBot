using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.SeedWork;
using System.Collections.Generic;

namespace HuntBot.Domain.HuntBotGames.Participants
{
    /// <summary>
    /// Encapsulates game participant information.
    /// </summary>
    public class HuntBotGameParticipant : Entity<int>
    {
        /// <summary>
        /// The participant's CitizenName.
        /// </summary>
        public string CitizenName { get; internal set; }

        /// <summary>
        /// The total number of game points that the participant has earned.
        /// </summary>
        public int GamePoints { get; internal set; }

        /// <summary>
        /// List of game objects, which a participant has found that the participant has found.
        /// </summary>
        public List<GameObjectFind> FoundObjects { get; internal set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HuntBotGameParticipant"/>.
        /// </summary>
        /// <param name="citizenNumber">The participant's CitizenNumber.</param>
        /// <param name="citizenName">The participant's CitizenName.</param>
        public HuntBotGameParticipant(int citizenNumber, string citizenName)
        {
            Id = citizenNumber;
            CitizenName = citizenName;
        }
    }
}