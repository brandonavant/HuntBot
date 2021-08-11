using HuntBot.Domain.HuntBotGames.HuntBotLocation;
using System;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    /// <summary>
    /// Encapsulates information used to create HuntBot instances in the Active Worlds universe.
    /// </summary>
    public record HuntBotConfig
    {
        /// <summary>
        /// The citizen number with which instances are created.
        /// </summary>
        public int CitizenNumber { get; private set; }

        /// <summary>
        /// The citizen number's privilege password.
        /// </summary>
        public string PrivilegePassword { get; private set; }

        /// <summary>
        /// The world location to which the instance will be created.
        /// </summary>
        public Location Location { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntBotConfig"/> class.
        /// </summary>
        private HuntBotConfig(int citizenNumber, string privilegePassword, Location location)
        {
            CitizenNumber = citizenNumber;
            PrivilegePassword = privilegePassword;
            Location = location;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HuntBotConfig"/> class for the given values.
        /// </summary>
        /// <param name="citizenNumber">The citizen number with which bot instances should be created.</param>
        /// <param name="privilegePassword">The privilege passwrod for the given citizen number.</param>
        /// <param name="location">The world and physical location where the bot will appear once logged in.</param>
        /// <returns></returns>
        public HuntBotConfig CreateHuntBotConfig(int citizenNumber, string privilegePassword, string location)
        {
            if (citizenNumber == 0 || string.IsNullOrEmpty(privilegePassword))
            {
                throw new ArgumentException("You must provide a citizen number and privilege password.");
            }

            if (!Location.TryParseLocation(location, out Location parsedLocation))
            {
                throw new LocationParseException("Invalid Location string.");
            }

            return new HuntBotConfig(citizenNumber, privilegePassword, parsedLocation);
        }
    }
}
