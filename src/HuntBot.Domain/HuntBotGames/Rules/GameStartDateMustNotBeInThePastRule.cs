using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that the given StartDate does not occur in the past.
    /// </summary>
    public class GameStartDateMustNotBeInThePastRule : IBusinessRule
    {
        /// <summary>
        /// The <see cref="DateTime"/> value to validate.
        /// </summary>
        private readonly DateTime _startDate;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "You must choose a start date that occurs in the future.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameStartDateMustNotBeInThePastRule"/>.
        /// </summary>
        /// <param name="startDate">The <see cref="DateTime"/> value to validate.</param>
        public GameStartDateMustNotBeInThePastRule(DateTime startDate)
        {
            _startDate = startDate;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _startDate < DateTime.UtcNow;
        }
    }
}
