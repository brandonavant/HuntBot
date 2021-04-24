using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.HuntBotConfiguration
{
    public record HuntBotSettings
    {
        public int CitizenNumber { get; set; }

        public string PrivilegePassword { get; set; }

        public string World { get; set; }
    }
}
