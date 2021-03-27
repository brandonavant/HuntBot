using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.SeedWork;
using System;
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
        public string CitizenName { get; private set; }

        /// <summary>
        /// The total number of game points that the participant has earned.
        /// </summary>
        public int GamePoints { get; private set; }

        /// <summary>
        /// List of game objects, which a participant has found that the participant has found.
        /// </summary>
        public List<GameObjectFind> ObjectFinds { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HuntBotGameParticipant"/>.
        /// </summary>
        /// <param name="eventApplier">Delegate used to perform a double-dispatch invocation to inform the <see cref="HuntBotGame"/> instance of changes.</param>
        public HuntBotGameParticipant(Action<object> eventApplier) : base(eventApplier) { }

        /// <summary>
        /// Matches an event to the event type and applies the corresponding changes to the <see cref="HuntBotGameParticipant"/> instance.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.HuntBotParticipantAdded e:
                    Id = e.CitizenNumber;
                    CitizenName = e.CitizenName;
                    ObjectFinds.Add(new GameObjectFind 
                    { 
                        ObjectId = e.FoundObjectId, 
                        FoundDate = DateTime.UtcNow, 
                        Points = e.Points 
                    });  
                    break;
                default:
                    break;
            }
        }
    }
}