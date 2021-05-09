using AW;
using HuntBot.Application.SaveHuntBotConfiguration;
using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using HuntBot.Domain.HuntBotGames.HuntBotLocation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuntBot.App
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Instance of the AW SDK.
        /// </summary>
        private IInstance _aw;

        /// <summary>
        /// Mediator with which commands and query requests and dispatched.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Used to log information and errors to the configured logging sink.
        /// </summary>
        private readonly ILogger<FrmMain> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmMain"/> class.
        /// </summary>
        /// <param name="mediator">Mediator with which commands and query requests and dispatched.</param>
        /// <param name="logger">Used to log information and errors to the configured logging sink.</param>
        public FrmMain(IMediator mediator, ILogger<FrmMain> logger)
        {
            try
            {
                _mediator = mediator;
                _logger = logger;
                _aw = new Instance();

                InitializeComponent();
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected exception occurred while attempting to initialize FrmMain.");
            }
        }

        /// <summary>2
        /// Event which fires upload loading an instance of <see cref="FrmMain"/>.
        /// </summary>
        /// <param name="sender">The construct that raised the Load event.</param>
        /// <param name="e">Encapsulates arguments passed into the Load event.</param>
        private async void FrmMain_Load(object sender, EventArgs e)
        {
            Location location;
            Domain.HuntBotGames.HuntBotLocation.Location.TryParseLocation("Droog 0n 0w 0a 0", out location);

            await _mediator.Send(new SaveHuntBotConfigurationCommand(
                new HuntBotConfig
                {
                    CitizenNumber = 339566,
                    Location = location,
                    PrivilegePassword = "ThisIsAPassword"
                })
            );
        }
    }
}
 