using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that a given participant wasn't already registered into the <see cref="HuntBotGame"/>
    /// </summary>
    public class ParticipantIsNotRegisteredInGameRule : IBusinessRule
    {
        /// <summary>
        /// The citizen number of the participant.
        /// </summary>
        private readonly int _citizenNumber;

        /// <summary>
        /// The unique identifier of the <see cref="HuntBotGame"/> instance.
        /// </summary>
        private readonly Guid _huntBotGameId;

        /// <summary>
        /// Provides a means of checking whether or not a participant has been registered into the game already.
        /// </summary>
        private readonly IParticipantUniquenessChecker _participantUniquenessChecker;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => $"The citizen number '{_citizenNumber}' has already been added to the game.";

        /// <summary>
        /// Initializes a new instance of <see cref="ParticipantIsNotRegisteredInGameRule"/>.
        /// </summary>
        /// <param name="citizenNumber">The citizen number of the participant.</param>
        public ParticipantIsNotRegisteredInGameRule(
            int citizenNumber,
            Guid huntBotGameId,
            IParticipantUniquenessChecker participantUniquenessChecker)
        {
            _citizenNumber = citizenNumber;
            _huntBotGameId = huntBotGameId;
            _participantUniquenessChecker = participantUniquenessChecker;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return !_participantUniquenessChecker.IsUnique(_huntBotGameId, _citizenNumber);
        }
    }
}
