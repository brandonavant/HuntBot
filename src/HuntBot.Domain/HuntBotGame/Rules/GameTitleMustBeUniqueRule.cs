using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGame.Rules
{
    /// <summary>
    /// Rule that ensures that the game title has not already been used.
    /// </summary>
    public class GameTitleMustBeUniqueRule : IBusinessRule
    {
        /// <summary>
        /// The game title whose uniqueness is being checked.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// Provides a means by which the uniqueness of a game title can be checked.
        /// </summary>
        private IGameUniquenessChecker _titleUniquenessChecker;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "The game title that you have chosen already exists.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameTitleMustBeUniqueRule"/>.
        /// </summary>
        /// <param name="title">The game title whose character length is being checked.</param>
        public GameTitleMustBeUniqueRule(string title, IGameUniquenessChecker titleUniquenessChecker)
        {
            _titleUniquenessChecker = titleUniquenessChecker;
        }

        /// <summary>
        /// Indicates whether or not a business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken() => !_titleUniquenessChecker.IsUnique(_title);
    }
}
