using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that the end date and time is subsequent to the start date and time.
    /// </summary>
    public class GameEndDateMustBeAfterStartDateRule : IBusinessRule
    {
        /// <summary>
        /// The <see cref="DateTime"/> value which represents the start date of the event.
        /// </summary>
        private readonly DateTime startDate;

        /// <summary>
        /// The <see cref="DateTime"/> value which represents the end date of the event.
        /// </summary>
        private readonly DateTime _endDate;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "You must choose an end date that occurs after the start date.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameEndDateMustBeAfterStartDateRule"/>
        /// </summary>
        /// <param name="endDate">The <see cref="DateTime"/> value to validate.</param>
        public GameEndDateMustBeAfterStartDateRule(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            _endDate = endDate;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _endDate <= startDate;
        }
    }
}
