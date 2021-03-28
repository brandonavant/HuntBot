using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames
{
    public record GameObjectFind
    {
        /// <summary>
        /// The Id of the game object.
        /// </summary>
        public int ObjectId { get; init; }

        /// <summary>
        /// The data and time in which the object was found.
        /// </summary>
        public DateTime FoundDate { get; init; }

        /// <summary>
        /// The number of game points that the find was worth.
        /// </summary>
        public int Points { get; init; }
    }
}
