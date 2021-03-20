using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Interface for business rules.
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>
        /// Indicates whether or not a business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        bool IsBroken();

        /// <summary>
        /// The message that is shown when the rule is broken.
        /// </summary>
        string ErrorMessage { get; }
    }
}
