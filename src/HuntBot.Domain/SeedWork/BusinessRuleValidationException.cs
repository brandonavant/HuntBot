using System;

namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Exception that is thrown when a <see cref="IBusinessRule"/> rule is broken.
    /// </summary>
    public class BusinessRuleValidationException : Exception
    {
        /// <summary>
        /// The rule that was broken.
        /// </summary>
        public IBusinessRule BrokenRule { get; set; }

        /// <summary>
        /// The details associated with the broken rule.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BusinessRuleValidationException"/>.
        /// </summary>
        public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.ErrorMessage)
        {
            BrokenRule = brokenRule;
            this.Details = brokenRule.ErrorMessage;
        }
        
        /// <summary>
        /// Converts the contents of the <see cref="BusinessRuleValidationException"/> to its string equivalent.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.ErrorMessage}";
        }
    }
}
