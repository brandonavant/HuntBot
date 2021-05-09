using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuntBot.Application.GetHuntBotConfiguration
{
    public class GetHuntBotConfigurationQueryHandler : IRequestHandler<GetHuntBotConfigurationQuery>
    {

        public GetHuntBotConfigurationQueryHandler()
        {
        }

        public Task<Unit> Handle(GetHuntBotConfigurationQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
