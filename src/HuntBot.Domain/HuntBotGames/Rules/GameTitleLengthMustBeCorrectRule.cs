using HuntBot.Domain.SeedWork;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that the game title that was chosen falls between 3 and 32 characters.
    /// </summary>
    public class GameTitleLengthMustBeCorrectRule : IBusinessRule
    {
        /// <summary>
        /// The game title whose character length is being checked.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "The game title must be between 3 and 32 characters.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameTitleLengthMustBeCorrectRule/>.
        /// </summary>
        /// <param name="title">The game title whose character length is being checked.</param>
        public GameTitleLengthMustBeCorrectRule(string title)
        {
            _title = title;
        }

        /// <summary>
        /// Indicates whether or not a business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _title.Length < 3 || _title.Length > 32;
        }
    }
}
