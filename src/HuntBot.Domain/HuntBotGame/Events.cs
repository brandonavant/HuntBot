using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGame
{
    /// <summary>
    /// Events that can occur against the <see cref="HuntBotGame" aggregate./>
    /// </summary>
    public static class Events
    {
        public class ParticipantAdded
        {
            /// <summary>
            /// The new participant's CitizenNumber.
            /// </summary>
            public int Id { get; init; }

            /// <summary>
            /// The new participant's CitizenName.
            /// </summary>
            public string Name { get; init; }

            /// <summary>
            /// The object that the player found, which initiated the newly created <see cref="HuntBotGameParticipant"/> instance.
            /// </summary>
            public HuntBotGameObject FoundObject { get; init; }
        }

        public class ParticipantFoundGameObject
        {

        }
    }
}
