using System.Collections.Generic;
using System.Linq;
using HuntBot.Domain.SeedWork;

namespace HuntBot.Domain.HuntBotGames.Rules
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
        /// Collection of game titles against which this game's uniqueness is checked.
        /// </summary>
        private List<string> _gameTitles;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "The game title that you have chosen already exists.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameTitleMustBeUniqueRule"/>.
        /// </summary>
        /// <param name="title">The game title whose character length is being checked.</param>
        public GameTitleMustBeUniqueRule(string title, List<string> gameTitles)
        {
            _title = title;
            _gameTitles = gameTitles;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken() => _gameTitles.Any(gt => gt == _title);
    }
}
