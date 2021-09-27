using HuntBot.Domain.HuntBotGames.HuntBotLocation;
using HuntBot.Domain.HuntBotGames.Rules;
using HuntBot.Domain.SeedWork;
using Newtonsoft.Json;
using System;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    /// <summary>
    /// Encapsulates information used to create HuntBot instances in the Active Worlds universe.
    /// </summary>
    public record HuntBotConfig
    {
        /// <summary>
        /// The AW Universe server host.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// The AW Universe server port.
        /// </summary>
        public int Port { get; }

        /// <summary>a
        /// The citizen number with which instances are created.
        /// </summary>
        public int CitizenNumber { get; }

        /// <summary>
        /// The citizen number's privilege password.
        /// </summary
        public string PrivilegePassword { get; }

        /// <summary>
        /// The name to associate with HuntBot game session and its data.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// The world location to which the instance will be created.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntBotConfig"/> class.
        /// </summary>
        [JsonConstructor]
        private HuntBotConfig(
            string host,
            int port,
            int citizenNumber,
            string privilegePassword,
            string gameName,
            Location location)
        {
            Host = host;
            Port = port;
            CitizenNumber = citizenNumber;
            PrivilegePassword = privilegePassword;
            GameName = gameName;
            Location = location;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HuntBotConfig"/> class for the given values.
        /// </summary>
        /// <param name="citizenNumber">The citizen number with which bot instances should be created.</param>
        /// <param name="privilegePassword">The privilege passwrod for the given citizen number.</param>
        /// <param name="location">The world and physical location where the bot will appear once logged in.</param>
        /// <returns></returns>
        public static HuntBotConfig CreateHuntBotConfig(
            string host,
            int port,
            int citizenNumber, 
            string privilegePassword,
            string gameName,
            string location
        )
        {
            CheckRule(new HuntBotConfigurationHasAllValuesRule(citizenNumber, privilegePassword, gameName, location));

            _ = Location.TryParseLocation(location, out Location parsedLocation);

            return new HuntBotConfig(
                host,
                port,                
                citizenNumber, 
                privilegePassword, 
                gameName,
                parsedLocation
            );
        }

        /// <summary>
        /// Provides aggregate validation by ensuring that the <see cref="IBusinessRule"/> is not broken.
        /// </summary>
        /// <param name="rule"><The <see cref="IBusinessRule"/> whose validity is checked./param>
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}
