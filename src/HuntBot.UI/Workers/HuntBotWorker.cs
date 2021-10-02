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
    internal sealed class HuntBotWorker
    {
        private readonly GameStateLookup _botStateLookup;
        private readonly IInstance _aw;

        public HuntBotWorker(GameStateLookup botStateLookup)
        {
            _botStateLookup = botStateLookup;
            _aw = new Instance();
        }
    }
}
