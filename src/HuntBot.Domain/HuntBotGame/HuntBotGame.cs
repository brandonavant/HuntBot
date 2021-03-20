using HuntBot.Domain.HuntBotGame.Rules;
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
        public List<HuntBotGameParticipant> Participants { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HuntBotGame"/>.
        /// </summary>
        /// <param name="id">The id of the newly created game..</param>
        /// <param name="title">The title of the newly-created game.</param>
        /// <param name="startDate">The date and time in which the game begins.</param>
        /// <param name="endDate">The date and time in which the game ends.</param>
        private HuntBotGame(Guid id, string title, DateTime startDate, DateTime endDate)
        {
            Participants = new List<HuntBotGameParticipant>();

            ApplyChange(new Events.HuntBotGameCreated
            {
                Id = id,
                Title = title,
                StartDate = startDate,
                EndDate = endDate
            });
        }

        /// <summary>
        /// Creates a new instance of <see cref="HuntBotGame"/>.
        /// </summary>
        /// <param name="title">The title of the game.</param>
        /// <param name="startDate">The date and time in which the game begins.</param>
        /// <param name="endDate">The date and time in which the game ends.</param>
        /// <returns></returns>
        public static HuntBotGame CreateNewHuntBotGame(
            Guid id,
            string title, 
            DateTime startDate, 
            DateTime endDate, 
            IGameUniquenessChecker gameUniquenessChecker
        )
        {
            CheckRule(new GameTitleLengthMustBeCorrectRule(title));
            CheckRule(new GameTitleMustBeUniqueRule(title, gameUniquenessChecker));
            CheckRule(new GameStartDateMustNotBeInThePastRule(startDate));

            return new HuntBotGame(id, title, startDate, endDate);
        }

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