using System;

namespace HuntBot.Domain.HuntBotGames.GameObjects
{
    public record GameObjectFind
    {
        /// <summary>
        /// The Id of the <see cref="GameObject"/>, which the participant found.
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// The data and time in which the object was found.
        /// </summary>
        public DateTime FoundDate { get; set; }
    }
}
