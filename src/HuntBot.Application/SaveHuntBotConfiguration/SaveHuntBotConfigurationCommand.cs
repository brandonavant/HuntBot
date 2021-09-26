using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;

namespace HuntBot.Application.SaveHuntBotConfiguration
{
    public class SaveHuntBotConfigurationCommand : IRequest<Unit>
    {
        /// <summary>
        /// The AW citizen number with which bot instances shall be created.
        /// </summary>
        public int CitizenNumber { get; }

        /// <summary>
        /// The AW privilege password with which bot instances shall be created.
        /// </summary>
        public string PrivilegePassword { get; }

        /// <summary>
        /// The physical location at which the created bot instances will appear.
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// Saves the given configuration values.
        /// </summary>
        /// <param name="config">The configuration values to be saved.</param>
        public SaveHuntBotConfigurationCommand(int citizenNumber, string privilegePassword, string location)
        {
            CitizenNumber = citizenNumber;
            PrivilegePassword = privilegePassword;
            Location = location;
        }

    }
}
