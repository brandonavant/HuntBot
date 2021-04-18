using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.SeedWork;
using MediatR;

namespace HuntBot.Application.CreateNewHuntBotGame
{
    public class CreateNewHuntBotGameCommandHandler : IRequestHandler<CreateNewHuntBotGameCommand, HuntBotGame>
    {
        private readonly IAggregateStore _aggregateStore;

        public CreateNewHuntBotGameCommandHandler(IAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }

        public async Task<HuntBotGame> Handle(CreateNewHuntBotGameCommand request, CancellationToken cancellationToken)
        {
            var existingGameTitles = (await _aggregateStore.LoadAll<HuntBotGame>())
                .Select(gt => gt.Title)
                .ToList();

            var huntBotGame = HuntBotGame.CreateNewHuntBotGame
            (
                Guid.NewGuid(), 
                request.Title, 
                request.StartDate, 
                request.EndDate, 
                existingGameTitles
            );

            return huntBotGame;
        }
    }
}