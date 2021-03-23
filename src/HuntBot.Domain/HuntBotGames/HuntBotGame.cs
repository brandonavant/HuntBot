using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.HuntBotGames.Rules;
using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace HuntBot.Domain.HuntBotGames
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
        /// The UTC in which the game starts.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// The UTC in which the game ends.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// List of game participants.
        /// </summary>
        public List<HuntBotGameParticipant> Participants { get; private set; }

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
        /// <param name="id">The unique identifier of the <see cref="HuntBotGame"/> instance.</param>
        /// <param name="title">The title of the game.</param>
        /// <param name="startDate">The date and time in which the game begins.</param>
        /// <param name="endDate">The date and time in which the game ends.</param>
        /// <param name="gameUniquenessChecker">Used to determine if the given game title is unique.</param>
        /// <returns>A newly-created instance of <see cref="HuntBotGame"/>.</returns>
        public static HuntBotGame CreateNewHuntBotGame(
            Guid id,
            string title, 
            DateTime startDate, 
            DateTime endDate, 
            IGameUniquenessChecker gameUniquenessChecker
        )
        {
            if (gameUniquenessChecker is null)
            {
                throw new ArgumentNullException(nameof(gameUniquenessChecker));
            }

            CheckRule(new GameTitleLengthMustBeCorrectRule(title));
            CheckRule(new GameTitleMustBeUniqueRule(title, gameUniquenessChecker));
            CheckRule(new GameStartDateMustNotBeInThePastRule(startDate));
            CheckRule(new GameEndDateMustBeAfterStartDateRule(startDate, endDate));

            return new HuntBotGame(id, title, startDate, endDate);
        }

        /// <summary>
        /// Adds a participant to the <see cref="Participants"/>.
        /// </summary>
        /// <param name="citizenNumber">The citizen number of the participant.</param>
        /// <param name="citizenName">The citizen name of the participant.</param>
        /// <param name="participantUniquenessChecker">Used to determine if the given citizen number has already been registered as a participant in this game.</param>
        /// <returns>A newly-created instance of <see cref="HuntBotGameParticipant"/>.</returns>
        public HuntBotGameParticipant AddParticipant(int citizenNumber, string citizenName, IParticipantUniquenessChecker participantUniquenessChecker)
        {
            HuntBotGameParticipant newParticipant;

            if (participantUniquenessChecker is null)
            {
                throw new ArgumentNullException(nameof(participantUniquenessChecker));
            }

            CheckRule(new ParticipantIsNotRegisteredInGameRule(citizenNumber, Id, participantUniquenessChecker));

            newParticipant = new HuntBotGameParticipant(citizenNumber, citizenName);
            Participants.Add(newParticipant);

            return newParticipant;
        }

        /// <summary>
        /// Matches a event to the event type and applies the corresponding changes to the aggregate.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.HuntBotGameCreated e:
                    Id = e.Id;
                    Title = e.Title;
                    StartDate = e.StartDate;
                    EndDate = e.EndDate;
                    break;
                case Events.ParticipantAdded e:
                    Participants.Add()
                default:
                    throw new NotImplementedException($"The event '{@event}' has not been implemented.");
            }
        }
    }
}