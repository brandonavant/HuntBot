using System;

namespace HuntBot.Domain.HuntBotGame
{
    public record HuntBotGameObjectFind
    {
        /// <summary>
        /// The Id of the <see cref="HuntBotGameObject"/>, which the participant found.
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// The data and time in which the object was found.
        /// </summary>
        public DateTime FoundDate { get; set; }
    }
}
