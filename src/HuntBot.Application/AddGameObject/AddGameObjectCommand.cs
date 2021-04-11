using MediatR;

namespace HuntBot.Application.AddGameObject
{
    /// <summary>
    /// Command used to add a game object to a HuntBot game instance.
    /// </summary>
    public class AddGameObjectCommand : IRequest
    {
        /// <summary>
        /// The unique identifier of the game object being added.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// The name of the world to which the game object is being added.
        /// </summary>
        public string WorldName { get; }

        /// <summary>
        /// The number of points to be awarded when a participant finds the game object.
        /// </summary>
        public int Points { get; set; }
    }
}