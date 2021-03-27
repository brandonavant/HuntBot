using System.Threading;
using System.Threading.Tasks;
using AW;
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
        /// Initializes a new instance of HuntBot.
        /// </summary>
        /// <param name="configuration"></param>
        public HuntBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Invoked wehn the HuntBot BackgroundService is started.
        /// </summary>
        /// <param name="stoppingToken">Token which signals that the operation should be terminated.</param>
        /// <returns><see cref="Task.CompletedTask"/> when the task is complete.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Read configuration and determine how many instances should be created (i.e. in how many worlds will the event be hosted -- up  to 5 worlds for now).
            // TODO: Create an instance per world in the event.
            // TODO: Determine if we are continuing a previous event (indicated by config file, for now).
            // TODO: Begin scanning each world. Once scanning is complete, store the results (i.e. egg count) in database.

            // TODO: When a user enters the world, check the CitizenName stored in the database for their CitizenNumber. If it is different, change it in the DB.

            return Task.CompletedTask;
        }
    }
}