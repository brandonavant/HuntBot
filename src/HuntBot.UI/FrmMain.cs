using AW;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
 