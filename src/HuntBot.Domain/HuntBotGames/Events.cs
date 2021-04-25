using System;

namespace HuntBot.Domain.HuntBotGames
{
    /// <summary>
    /// Events that can occur against the <see cref="HuntBotGame" aggregate./>
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Aggregate event that is raised when a new <see cref="HuntBotGame" is created./>
        /// </summary>
        public class HuntBotGameCreated
        {
            /// <summary>
            /// The Id of the newly-created game.
            /// </summary>
            public Guid Id { get; init; }

            /// <summary>
            /// The title of the newly-created game.
            /// </summary>
            public string Title { get; init; }

            /// <summary>
            /// The UTC in which the game starts.
            /// </summary>
            public DateTime StartDate { get; init; }

            /// <summary>
            /// The UTC in which the game ends.
            /// </summary>
            public DateTime EndDate { get; init; }
        }

        /// <summary>
        /// Aggregate event that is raised when a game participant is added.
        /// </summary>
        public class ParticipantAdded
        {
            /// <summary>
            /// The new participant's CitizenNumber.
            /// </summary>
            public int CitizenNumber { get; init; }

            /// <summary>
            /// The new participant's CitizenName.
            /// </summary>
            public string CitizenName { get; init; }

            /// <summary>
            /// The Id of the object that was found.
            /// </summary>
            public int FoundObjectId { get; init; }

            /// <summary>
            /// The number of game points that the find is worth.
            /// </summary>
            public int Points { get; init; }
        }

        /// <summary>
        /// Aggregate event that is raised when a game participant finds a game object.
        /// </summary>
        public class ParticipantFoundGameObject
        {
            /// <summary>
            /// The Id of the game object that was found.
            /// </summary>
            public int FoundObjectId { get; init; }

            /// <summary>
            /// The number of game points that the find is worth.
            /// </summary>
            public int Points { get; init; }
        }

        /// <summary>
        /// Aggregate event that is raised when a game object is added.
        /// </summary>
        public class GameObjectAdded
        {
            /// <summary>
            /// The object's unique identifier.
            /// </summary>
            public int ObjectId { get; init; }

            /// <summary>
            /// The world to which the object is being added.
            /// </summary>
            public string WorldName { get; init; }

            /// <summary>
            /// The number of points to be awarded when this object is found.
            /// </summary>
            public int Points { get; init; }
        }
    }
}
