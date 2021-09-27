using HuntBot.Domain.HuntBotGames.HuntBotLocation;
using HuntBot.Domain.SeedWork;
namespace HuntBot.Domain.HuntBotGames.Rules
{
    internal class HuntBotConfigurationHasAllValuesRule : IBusinessRule
    {
        private readonly int _citizenNumber;
        private readonly string _privilegePassword;
        private readonly string _gameName;
        private readonly string _location;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => "You must provide values for each of the required configuration fields (i.e. Citizen Number, Privilege Password, and Game Name).";

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntBotConfigurationHasAllValuesRule"/> class.
        /// </summary>
        /// <param name="citizenNumber">The citizen number with which instances are created.</param>
        /// <param name="privilegePassword">The citizen number's privilege password.</param>
        /// <param name="gameName">The name to associate with HuntBot game session and its data.</param>
        /// <param name="location">The world location to which the instance will be created.</param>
        public HuntBotConfigurationHasAllValuesRule(
            int citizenNumber,
            string privilegePassword,
            string gameName,
            string location
        )
        {
            _citizenNumber = citizenNumber;
            _privilegePassword = privilegePassword;
            _gameName = gameName;
            _location = location;
        }

        /// <summary>
        /// Indicates whether or not a business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return _citizenNumber <= 0 || string.IsNullOrEmpty(_privilegePassword) || string.IsNullOrEmpty(_gameName) || !Location.TryParseLocation(_location, out Location _);
        }
    }
}