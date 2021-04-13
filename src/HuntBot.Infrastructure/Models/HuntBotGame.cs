using System;

namespace HuntBot.Infrastructure.Models
{
    public class HuntBotGame
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public string Store { get; set; }
    }
}