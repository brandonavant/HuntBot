using System.Collections.Generic;
using System.Linq;
using HuntBot.Domain.SeedWork;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that a participant can only find the same game object once.
    /// </summary>
    public class ParticipantHasNotFoundObjectAlreadyRule : IBusinessRule
    {
        /// <summary>
        /// The participant's citizen number.
        /// </summary>
        private readonly int _participantId;

        /// <summary>
        /// The object's id.
        /// </summary>
        private readonly int _objectId;

        /// <summary>
        /// A list of object ids of the game objects that the participant has already found.
        /// </summary>
        private readonly IEnumerable<int> _participantFinds;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => throw new System.NotImplementedException();

        /// <summary>
        /// Initializes a new instance of <see cref="ParticipantHasNotFoundObjectAlreadyRule"/>.
        /// </summary>
        /// <param name="participantId">The participant's citizen number.</param>
        /// <param name="objectId">The object's id.</param>
        /// <param name="participantFinds">A list of object ids of the game objects that the participant has already found.</param>
        public ParticipantHasNotFoundObjectAlreadyRule(int participantId, int objectId, IEnumerable<int> participantFinds)
        {
            _participantId = participantId;
            _objectId = objectId;
            _participantFinds = participantFinds;
        }
        
        /// <summary>
        /// Indicates whether or not a business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _participantFinds.Any(pf => pf == _objectId);
        }
    }
}