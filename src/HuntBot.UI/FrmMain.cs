using AW;
using HuntBot.Application.GetHuntBotConfiguration;
using HuntBot.Application.SaveHuntBotConfiguration;
using HuntBot.Domain.HuntBotGames.GameState;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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
        /// ConcurrentDictionary whcih provides a global state across multiple threads with which processes can check the state of a running HuntBot game session.
        /// </summary>
        private readonly GameStateLookup _gameStateLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmMain"/> class.
        /// </summary>
        /// <param name="mediator">Mediator with which commands and query requests and dispatched.</param>
        /// <param name="logger">Used to log information and errors to the configured logging sink.</param>
        public FrmMain(IMediator mediator, ILogger<FrmMain> logger, GameStateLookup gameStateLookup)
        {
            try
            {
                _mediator = mediator;
                _logger = logger;
                _aw = new Instance();
                _gameStateLookup = gameStateLookup;

                InitializeComponent();
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected exception occurred while attempting to initialize FrmMain.");
            }

            _gameStateLookup = gameStateLookup;
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

                txtHost.Text = configuration.Host;
                txtPort.Text = configuration.Port.ToString();
                txtCitizenNumber.Text = configuration.CitizenNumber.ToString();
                txtPrivilegePassword.Text = configuration.PrivilegePassword;
                txtGameName.Text = configuration.GameName;
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

                if (!int.TryParse(txtPort.Text, out int port))
                {
                    throw new ArgumentException("You must enter ");
                }

                await _mediator.Send(new SaveHuntBotConfigurationCommand(txtHost.Text, port, citizenNumber, txtPrivilegePassword.Text, txtGameName.Text, txtGameLocation.Text));

                MessageBox.Show("Save Successful!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to save HuntBot configuration.");
            }
        }
    }
}
 