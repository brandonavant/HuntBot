using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.SeedWork;
using MediatR;

namespace HuntBot.Application.GetHuntBotGames
{
    public class GetHuntBotGamesQueryHandler : IRequestHandler<GetHuntBotGamesQuery, List<HuntBotGame>>
    {
        private readonly IAggregateStore _aggregateStore;

        public GetHuntBotGamesQueryHandler(IAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }
        public async Task<List<HuntBotGame>> Handle(GetHuntBotGamesQuery request, CancellationToken cancellationToken)
        {
            return await _aggregateStore.LoadAll<HuntBotGame>();
        }
    }
}