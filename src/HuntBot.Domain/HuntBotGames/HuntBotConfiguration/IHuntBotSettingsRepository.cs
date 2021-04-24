using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    public interface IHuntBotSettingsRepository
    {
        /// <summary>
        /// Saves the given <see cref="HuntBotSettings"/> instance.
        /// </summary>
        /// <param name="settings">The <see cref="HuntBotSettings"/> instance to be saved.</param>
        /// <returns>True if the save was successful; false otherwise.</returns>
        Task<bool> SaveSettings(HuntBotSettings settings);

        /// <summary>
        /// Loads the <see cref="HuntBotSettings"/> instance.
        /// </summary>
        /// <returns>The loaded <see cref="HuntBotSettings"/> instance.</returns>
        Task<HuntBotSettings> LoadSettings();
    }
}
