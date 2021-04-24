using System;
using System.Threading;
using System.Threading.Tasks;
using AW;
using HuntBot.App.Configuration;
using HuntBot.Application.CreateNewHuntBotGame;
using HuntBot.Application.GetHuntBotGames;
using HuntBot.Domain.HuntBotGames;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

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
        private readonly HuntBotSettings _gameConfig;

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
            _mediator = mediator;
            _aw = new Instance();

            configuration.Bind("HuntBotSettings", _gameConfig);

            ValidateAppSettings();
        }

        /// <summary>
        /// Invoked wehn the HuntBot BackgroundService is started.
        /// </summary>
        /// <param name="stoppingToken">Token which signals that the operation should be terminated.</param>
        /// <returns><see cref="Task.CompletedTask"/> when the task is complete.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var existingHuntBotGames = await _mediator.Send(new GetHuntBotGamesQuery());

            // Present to the user a list of existing games AND give them the option to create a new one
            // If they choose an existing, start the game; if they choose to create a new once, take them through the prompts to get data that's needed to create a new one.
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

        /// <summary>
        /// Ensures that the user has completed the 'HuntBotSettings' section of appsettings.json.
        /// </summary>
        private void ValidateAppSettings()
        {
            string errorMessage;

            if (_gameConfig is null)
            {
                errorMessage = "Unable to initialize. The appsettings.json file is missing the 'HuntBotSettings' section.";
                Log.Logger.Fatal(errorMessage);

                throw new NullReferenceException(errorMessage);
            }

            if (_gameConfig.CitizenNumber == 0 || string.IsNullOrEmpty(_gameConfig.PrivilegePassword) || string.IsNullOrEmpty( _gameConfig.World))
            {
                errorMessage = "Unable to initialize. The 'HuntBotSettings' section of appsettings.json is incomplete.";
                Log.Logger.Fatal(errorMessage);

                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}