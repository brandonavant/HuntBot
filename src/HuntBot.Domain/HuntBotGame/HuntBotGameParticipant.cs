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
        /// List of game objects, which a participant has found that the participant has found.
        /// </summary>
        public List<HuntBotGameObject> FoundObjects { get; set; }
    }
}