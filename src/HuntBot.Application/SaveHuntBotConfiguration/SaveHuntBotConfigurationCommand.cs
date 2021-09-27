using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;

namespace HuntBot.Application.SaveHuntBotConfiguration
{
    public class SaveHuntBotConfigurationCommand : IRequest<Unit>
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
        /// The physical location at which the created bot instances will appear.
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// Saves the given configuration values.
        /// </summary>
        /// <param name="config">The configuration values to be saved.</param>
        public SaveHuntBotConfigurationCommand(
            string host,
            int port,
            int citizenNumber,
            string privilegePassword,
            string gameName,
            string location
        )
        {
            Host = host;
            Port = port;
            CitizenNumber = citizenNumber;
            PrivilegePassword = privilegePassword;
            GameName = gameName;
            Location = location;
        }

    }
}
