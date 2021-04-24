using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    public interface ILocationParser
    {
        /// <summary>
        /// Domain service which attempts to parse a <see cref="Location"/> instance out of a formatted string.
        /// </summary>
        /// <param name="location">The string representation of the <see cref="Location"/>.</param>
        /// <returns>An initialized instance of <see cref="Location"/>.</returns>
        /// <exception cref="LocationParseException">Thrown when the domain service is unable to parse the given location string.</exception>
        Task<Location> ParseLocation(string location);
    }
}
