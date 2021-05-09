using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HuntBot.Application.SaveHuntBotConfiguration
{
    public class SaveHuntBotConfigurationCommandHandler : IRequestHandler<SaveHuntBotConfigurationCommand, Unit>
    {
        /// <summary>
        /// Repository with which the configuration values are saved to the persistence layer.
        /// </summary>
        private readonly IHuntBotConfigRepository _huntBotConfigRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveHuntBotConfigurationCommandHandler"/> class.
        /// </summary>
        /// <param name="huntBotConfigRepository">Repository with which the configuration values are saved to the persistence layer.</param>
        public SaveHuntBotConfigurationCommandHandler(IHuntBotConfigRepository huntBotConfigRepository)
        {
            _huntBotConfigRepository = huntBotConfigRepository;
        }

        /// <summary>
        /// Handles <see cref="SaveHuntBotConfigurationCommand"/> requests.
        /// </summary>
        /// <param name="request">The <see cref="SaveHuntBotConfigurationCommand"/> command which triggered this handler.</param>
        /// <param name="cancellationToken">The signal with which the operation is cancelled.</param>
        public async Task<Unit> Handle(SaveHuntBotConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (request.Config is null)
            {
                throw new ArgumentException("The config value cannot be null.");
            }

            await _huntBotConfigRepository.SaveSettings(request.Config);

            return Unit.Value;
        }
    }
}
