using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace HuntBot.Domain.HuntBotGame
{
    /// <summary>
    /// Encapsulates HuntBot game information.
    /// </summary>
    public class HuntBotGame : AggregateRoot
    {
        /// <summary>
        /// The title of the HuntBot game.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The VRT date and time in which the game starts.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// The VRT date and time in which the game ends.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// List of game participants.
        /// </summary>
        public List<HubtBotGameParticipant> Participants { get; set; }

        /// <summary>
        /// Matches a event to the event type and applies the corresponding changes to the aggregate.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}