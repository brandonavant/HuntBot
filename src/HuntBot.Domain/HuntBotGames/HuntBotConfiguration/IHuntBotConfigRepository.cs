using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    public interface IHuntBotConfigRepository
    {
        /// <summary>
        /// Saves the given <see cref="HuntBotConfig"/> instance.
        /// </summary>
        /// <param name="settings">The <see cref="HuntBotConfig"/> instance to be saved.</param>
        /// <returns>True if the save was successful; false otherwise.</returns>
        Task<bool> SaveSettings(HuntBotConfig settings);

        /// <summary>
        /// Loads the <see cref="HuntBotConfig"/> instance.
        /// </summary>
        /// <returns>The loaded <see cref="HuntBotConfig"/> instance.</returns>
        Task<HuntBotConfig> LoadSettings();
    }
}
