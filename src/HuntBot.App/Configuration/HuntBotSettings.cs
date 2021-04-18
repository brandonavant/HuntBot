namespace HuntBot.App.Configuration
{
    /// <summary>
    /// Encapsulates HuntBot configuration properties, which are part of appsettings.json.
    /// </summary>
    public class HuntBotSettings
    {
        /// <summary>
        /// The AW citizen number with which HuntBot instances are created.
        /// </summary>
        public int CitizenNumber { get; set; }

        /// <summary>
        /// The AW citizen privilege password with which HuntBot instances are created.
        /// </summary>
        public string PrivilegePassword { get; set; }

        /// <summary>
        /// The world in which the bot enters and hosts a game session.
        /// </summary>
        /// <value></value>
        public string World { get; set; }
    }
}