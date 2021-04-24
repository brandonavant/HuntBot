using System;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    public class LocationParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationParseException"/> class.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        public LocationParseException(string message) : base(message)
        {
        }
    }
}
