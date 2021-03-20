namespace HuntBot.Domain.HuntBotGame
{
    /// <summary>
    /// Events that can occur against the <see cref="HuntBotGame" aggregate./>
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Aggregate event that is raised when a game participant is added.
        /// </summary>
        public class ParticipantAdded
        {
            /// <summary>
            /// The new participant's CitizenNumber.
            /// </summary>
            public int Id { get; init; }

            /// <summary>
            /// The new participant's CitizenName.
            /// </summary>
            public string Name { get; init; }

            /// <summary>
            /// The object that the player found, which initiated the newly created <see cref="HuntBotGameParticipant"/> instance.
            /// </summary>
            public HuntBotGameObject FoundObject { get; init; }
        }

        /// <summary>
        /// Aggregate event that is raised when a game participant finds a game object.
        /// </summary>
        public class ParticipantFoundGameObject
        {
            /// <summary>
            /// The Id of the game object that was found.
            /// </summary>
            public int ObjectId { get; init; }

            /// <summary>
            /// The Id of the participant.
            /// </summary>
            public int ParticipantId { get; set; }
        }
    }
}
