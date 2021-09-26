using AW;
using HuntBot.Application.GetHuntBotConfiguration;
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

        /// <summary>
        /// Event handler which fires upload loading an instance of <see cref="FrmMain"/>.
        /// </summary>
        /// <param name="sender">The construct that raised the Load event.</param>
        /// <param name="e">Encapsulates arguments passed into the Load event.</param>
        private async void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                var configuration = await _mediator.Send(new GetHuntBotConfigurationQuery());

                txtCitizenNumber.Text = configuration.CitizenNumber.ToString();
                txtPrivilegePassword.Text = configuration.PrivilegePassword;
                txtGameLocation.Text = configuration.Location.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to load configuration file.", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RtbSay_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(rtbSay.Text))
            {
                _aw.SayChannel(ChatChannels.Global, rtbSay.Text);
            }
        }

        /// <summary>
        /// Handles the button click event for <see cref="btnLogin"/>, which initiates
        /// the login process for the given credentials and location.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">Encapsulates event data.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the button click event for <see cref="btnSave"/>, which initiates
        /// the process to save the current state of the configuration fields.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">Encapsulates event data.</param>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtCitizenNumber.Text, out var citizenNumber))
                {
                    throw new ArgumentException("You must enter a valid citizen number.");
                }

                await _mediator.Send(new SaveHuntBotConfigurationCommand(citizenNumber, txtPrivilegePassword.Text, txtGameLocation.Text));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to save HuntBot configuration.");
            }
        }
    }
}
 