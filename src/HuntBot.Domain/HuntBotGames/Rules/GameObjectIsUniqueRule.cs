using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Domain.HuntBotGames.Rules
{
    /// <summary>
    /// Rule that ensures that a given GameId and ObjectId combination is unique.
    /// </summary>
    public class GameObjectIsUniqueRule : IBusinessRule
    {
        /// <summary>
        /// The object id of the game object whose uniqueness is being checked.
        /// </summary>
        private readonly int _objectId;

        /// <summary>
        /// The game id of the game for which the object's uniqueness is being checked.
        /// </summary>
        private readonly Guid _gameId;

        /// <summary>
        /// Resource with which an object id's uniqueness is checked.
        /// </summary>
        private readonly IObjectUniquenessChecker _objectUniquenessChecker;

        /// <summary>
        /// The error message to be displayed to the user.
        /// </summary>
        public string ErrorMessage => $"An object with the id '{_objectId}' has already been added to the game.";

        /// <summary>
        /// Initializes a new instance of <see cref="GameObjectIsUniqueRule"/>.
        /// </summary>
        /// <param name="objectId">The error message to be displayed to the user.</param>
        /// <param name="objectUniquenessCHecker">Resource with which an object id's uniqueness is checked.</param>
        public GameObjectIsUniqueRule(Guid gameId, int objectId, IObjectUniquenessChecker objectUniquenessCHecker)
        {
            _objectId = objectId;
            _gameId = gameId;
        }

        /// <summary>
        /// Indicates whether or not the business rule is broken.
        /// </summary>
        /// <returns>True if the rule is broken.</returns>
        public bool IsBroken()
        {
            return !_objectUniquenessChecker.IsUnique(_gameId, _objectId);
        }
    }
}
