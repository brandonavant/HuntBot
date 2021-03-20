using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGame.Rules
{
    /// <summary>
    /// Rule that ensures that the end date and time is subsequent to the start date and time.
    /// </summary>
    public class GameEndDateMustBeAfterStartDateRule
    {

        public GameEndDateMustBeAfterStartDateRule(DateTime endDate)
        {

        }
    }
}
