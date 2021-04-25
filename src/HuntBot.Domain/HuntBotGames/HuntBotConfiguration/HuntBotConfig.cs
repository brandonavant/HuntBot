using HuntBot.Domain.HuntBotGames.HuntBotLocation;

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
        public int CitizenNumber { get; set; }

        /// <summary>
        /// The citizen number's privilege password.
        /// </summary>
        public string PrivilegePassword { get; set; }

        /// <summary>
        /// The world in which the newly created instance will enter.
        /// </summary>
        public string World { get; set; }

        /// <summary>
        /// The world location to which the instance will be created.
        /// </summary>
        public Location Location { get; set; }
    }
}
