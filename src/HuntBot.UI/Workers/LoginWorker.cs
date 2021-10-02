using AW;
using HuntBot.Domain.HuntBotGames.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.UI.Workers
{
    /// <summary>
    /// Background worker tasked with logging in an instance into AW.
    /// </summary>
    internal sealed class LoginWorker
    {
        private readonly BotStateLookup _botStateLookup;
        private readonly IInstance _awInstance;

        public LoginWorker(BotStateLookup botStateLookup, IInstance awInstance)
        {
            _botStateLookup = botStateLookup;
            _awInstance = awInstance;
        }
    }
}
