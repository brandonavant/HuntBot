using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Collection of current <see cref="GameParticipant"/> instances against which a participant's existence is checked.
        /// </summary>
        private readonly List<GameParticipant> _gameParticipants;

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
            List<GameParticipant> gameParticipants)
        {
            _citizenNumber = citizenNumber;
            _huntBotGameId = huntBotGameId;
            _gameParticipants = gameParticipants;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _gameParticipants.Any(gp => gp.Id == _citizenNumber);
        }
    }
}
