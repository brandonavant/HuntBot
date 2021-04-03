using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.GameObjects
{
    public interface IObjectUniquenessChecker
    {
        /// <summary>
        /// Determines if the given game id has already been registered in the game.
        /// </summary>
        /// <param name="gameId">The game id for which the object's registration is checked.</param>
        /// <param name="objectId">The id of the object whose registration in the game is checked.</param>
        /// <returns>True if the object has been registered in the game.</returns>
        bool IsUnique(Guid gameId, int objectId);
    }
}
