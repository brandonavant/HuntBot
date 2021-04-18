using System.Collections.Generic;
using HuntBot.Domain.HuntBotGames;
using MediatR;

namespace HuntBot.Application.GetHuntBotGames
{
    public class GetHuntBotGamesQuery : IRequest<List<HuntBotGame>>
    {
    }
}