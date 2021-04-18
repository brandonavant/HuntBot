using System;
using System.Threading;
using System.Threading.Tasks;
using AW;
using HuntBot.Application.CreateNewHuntBotGame;
using HuntBot.Domain.HuntBotGames;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HuntBot.App
{
    public class HuntBot : BackgroundService
    {
        /// <summary>
        /// Instance of the AW SDK.
        /// </summary>
        private IInstance _aw;

        /// <summary>
        /// Configuration settings for creating bot instances in the Active Worlds universe.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Mediator with which commands and query requests and dispatched.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of HuntBot.
        /// </summary>
        /// <param name="configuration"></param>
        public HuntBot(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Invoked wehn the HuntBot BackgroundService is started.
        /// </summary>
        /// <param name="stoppingToken">Token which signals that the operation should be terminated.</param>
        /// <returns><see cref="Task.CompletedTask"/> when the task is complete.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Check the configuration for the HuntBotGame configuration. The configuration should only care about the game name name, citnum, bot name, privpass, and world.
            // If the configuration isn't right, present an error.
            // if the configuration is right, create a new HuntBotGame and start the bot.
            // Announce to the world that we are preparing the game; block any finds until querying is complete
            // Start querying the world, adding a GameObject for each object found
            // If an Add throws an exception indicating that it was already added, log it and move on
            // Once querying is complete, announce to the world that the game is started.
            // Begin tracking "finds"
            // Whisper to a player when they've found a new object; if it was already found, let them know that they've already found it.

            // TODO: When a user enters the world, check the CitizenName stored in the database for their CitizenNumber. If it is different, change it in the DB.
            // var newHuntBotGame = await _mediator.Send<HuntBotGame>(
            //     new CreateNewHuntBotGameCommand("AppLevelTest", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            // );

            while(!stoppingToken.IsCancellationRequested && Utility.Wait(-1) != ReasonCode.Success);
        }
    }
}