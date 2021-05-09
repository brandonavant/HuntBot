using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;

namespace HuntBot.Application.SaveHuntBotConfiguration
{
    public class SaveHuntBotConfigurationCommand : IRequest<Unit>
    {
        /// <summary>
        /// The configuration values to be saved.
        /// </summary>
        public HuntBotConfig Config { get; }

        /// <summary>
        /// Saves the given configuration values.
        /// </summary>
        /// <param name="config">The configuration values to be saved.</param>
        public SaveHuntBotConfigurationCommand(HuntBotConfig config)
        {
            Config = config;
        }
    }
}
