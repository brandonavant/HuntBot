using AW;
using HuntBot.Application.GetHuntBotConfiguration;
using HuntBot.Application.SaveHuntBotConfiguration;
using HuntBot.Domain.HuntBotGames.GameState;
using HuntBot.Domain.HuntBotGames.HuntBotConfiguration;
using HuntBot.Domain.HuntBotGames.HuntBotLocation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using HuntBotLocation = HuntBot.Domain.HuntBotGames.HuntBotLocation.Location;

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
        /// The phsyical location at which the bot will appear once logged in.
        /// </summary>
        private HuntBotConfig _huntbotConfiguration;

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
                //WireUpEventHandlers();
                WireUpCallbacks();
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
                _huntbotConfiguration = await _mediator.Send(new GetHuntBotConfigurationQuery());

                if (_huntbotConfiguration != null)
                {
                    txtHost.Text = _huntbotConfiguration.Host;
                    txtPort.Text = _huntbotConfiguration.Port.ToString();
                    txtCitizenNumber.Text = _huntbotConfiguration.CitizenNumber.ToString();
                    txtPrivilegePassword.Text = _huntbotConfiguration.PrivilegePassword;
                    txtGameName.Text = _huntbotConfiguration.GameName;
                    txtGameLocation.Text = _huntbotConfiguration.Location.ToString();
                }

            }
            catch (Exception ex)
            {
                var errorMessage = "Unable to load configuration file.";

                rtbChat.AppendText(errorMessage);
                _logger.LogError(errorMessage, ex);
            }
        }

        /// <summary>
        /// Wires up all <see cref="AW.AW_EVENT"/> event handlers.
        /// </summary>
        private void WireUpCallbacks()
        {
            _aw.CallbackLogin += CallbackLogin;
            _aw.CallbackEnter += CallbackEnter;
        }

        /// <summary>
        /// Wires up all <see cref="AW.AW_CALLBACK"/> callback handlers.
        /// </summary>
        private void WireUpEventHandlers()
        {
            throw new NotImplementedException();
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
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var configuration = await SaveConfiguration();

                _aw.Attributes.LoginOwner = configuration.CitizenNumber;
                _aw.Attributes.LoginPrivilegePassword = configuration.PrivilegePassword;
                _aw.Attributes.LoginApplication = this.ProductName;
                _aw.Attributes.LoginName = "Huntbot";

                var result = _aw.Login();

                while (Utility.Wait(-1) != ReasonCode.Success);
            }
            catch (Exception ex) 
            {
                // TODO: Clean this up.
                rtbChat.AppendText(ex.Message);
                _logger.LogError(ex, ex.Message); 
            }
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
                _huntbotConfiguration = await SaveConfiguration();

                MessageBox.Show("Save Successful!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to save HuntBot configuration.");
                rtbChat.AppendText("Failed to save.");
            }
        }

        private async Task<HuntBotConfig> SaveConfiguration()
        {
            if (!int.TryParse(txtCitizenNumber.Text, out var citizenNumber))
            {
                throw new ArgumentException("You must enter a valid citizen number.");
            }

            if (!int.TryParse(txtPort.Text, out int port))
            {
                throw new ArgumentException("You must enter a valid port.");
            }

            return await _mediator.Send(new SaveHuntBotConfigurationCommand(txtHost.Text, port, citizenNumber, txtPrivilegePassword.Text, txtGameName.Text, txtGameLocation.Text));
        }

        /// <summary>
        /// Callback handler which processes a <see cref="AW.AW_CALLBACK.AW_CALLBACK_LOGIN"/> callback.
        /// </summary>
        /// <param name="sender">The <see cref="AW.IInstance"/> instance which triggered the callback.</param>
        /// <param name="reasonCode">The <see cref="AW.ReasonCode"/> result of the call to <see cref="AW.IInstance.Login"/>.</param>
        private void CallbackLogin(IInstance sender, ReasonCode reasonCode)
        {
            if (reasonCode != ReasonCode.Success)
            {
                var errorMessage = $"Failed to login (reason {reasonCode})";

                rtbChat.AppendText(errorMessage);
                _logger.LogError(errorMessage);
            }

            _aw.Enter(_huntbotConfiguration.Location.World);
        }

        /// <summary>
        /// Callback handler which prcoesses a <see cref="AW.AW_CALLBACK.AW_CALLBACK_ENTER"/> callback.
        /// </summary>
        /// <param name="sender">The <see cref="AW.IInstance"/> instance which triggered the callback.</param>
        /// <param name="reasonCode">The <see cref="AW.ReasonCode"/> result of the call to <see cref="AW.IInstance.Enter()"/>.</param>
        private void CallbackEnter(IInstance sender, ReasonCode reasonCode)
        {
            ReasonCode stateChangeReasonCode = ReasonCode.Success;

            if (reasonCode != ReasonCode.Success)
            {
                throw new Exception($"Failed to enter {_huntbotConfiguration.Location.World} (reason {reasonCode})");
            }

            rtbChat.AppendText($"Successfully entered world {_huntbotConfiguration.Location.World}!");

            _aw.Attributes.MyX = _huntbotConfiguration.Location.X;
            _aw.Attributes.MyY = _huntbotConfiguration.Location.Y;
            _aw.Attributes.MyZ = _huntbotConfiguration.Location.Z;
            _aw.Attributes.MyYaw = _huntbotConfiguration.Location.Yaw;

            stateChangeReasonCode = _aw.StateChange();
            if (stateChangeReasonCode != ReasonCode.Success)
            {
                throw new Exception($"Failed to change state (reason {stateChangeReasonCode})");
            }
        }
    }
}
 