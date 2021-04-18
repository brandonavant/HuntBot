using System;
using HuntBot.Domain.HuntBotGames;
using MediatR;

namespace HuntBot.Application.CreateNewHuntBotGame
{
    public class CreateNewHuntBotGameCommand : IRequest<HuntBotGame>
    {
        public string Title { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        
        public CreateNewHuntBotGameCommand(string title, DateTime startDate, DateTime endDate)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}