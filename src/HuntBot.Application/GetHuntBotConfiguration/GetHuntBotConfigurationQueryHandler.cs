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
    public class GetHuntBotConfigurationQueryHandler : IRequestHandler<GetHuntBotConfigurationQuery, HuntBotConfig>
    {
        private readonly IHuntBotConfigRepository _huntBotConfigRepository;

        public GetHuntBotConfigurationQueryHandler(IHuntBotConfigRepository huntBotConfigRepository)
        {
            _huntBotConfigRepository = huntBotConfigRepository;
        }

        public async Task<HuntBotConfig> Handle(GetHuntBotConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await _huntBotConfigRepository.LoadSettings();
        }
    }
}
