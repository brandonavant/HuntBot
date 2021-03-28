using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    public class ParticipantIsRegisteredInGameRule : IBusinessRule
    {
        /// <summary>
        /// The participant for which registration is being checked.
        /// </summary>
        private HuntBotGameParticipant _participant;

        /// <summary>
        /// The error message displayed when the rule is broken.
        /// </summary>
        public string ErrorMessage => $"A participant with the CitizenNumber '{_participant.Id} is not currently registered in this game session.";

        /// <summary>
        /// Initializes a new instance of <see cref="ParticipantIsRegisteredInGameRule"/>.
        /// </summary>
        /// <param name="participant">The participant for which registration is being checked.</param>
        public ParticipantIsRegisteredInGameRule(HuntBotGameParticipant participant)
        {
            _participant = participant;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _participant is null;
        }
    }
}
