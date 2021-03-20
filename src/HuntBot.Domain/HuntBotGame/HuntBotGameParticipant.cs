using HuntBot.Domain.SeedWork;
using System.Collections.Generic;

namespace HuntBot.Domain.HuntBotGame
{
    /// <summary>
    /// Encapsulates game participant information.
    /// </summary>
    public class HuntBotGameParticipant : Entity<int>
    {
        /// <summary>
        /// The participant's CitizenName.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The total number of game points that the participant has earned.
        /// </summary>
        public int GamePoints { get; set; }

        /// <summary>
        /// List of game objects, which a participant has found that the participant has found.
        /// </summary>
        public List<HuntBotGameObjectFind> FoundObjects { get; set; }
    }
}