using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;

namespace HuntBot.Application.GetHuntBotConfiguration
{
    public class GetHuntBotConfigurationQuery : IRequest<HuntBotConfig>
    {
        public GetHuntBotConfigurationQuery()
        {
        }
    }
}
