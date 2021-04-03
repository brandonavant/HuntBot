using HuntBot.Domain.SeedWork;
using System;

namespace HuntBot.Domain.HuntBotGames.GameObjects
{
    /// <summary>
    /// Encapsulates game object information.
    /// </summary>
    public class GameObject : Entity<int>
    {
        /// <summary>
        /// The number of points to be awarded to a participant upon finding this object.
        /// </summary>
        public int Points { get; init; }

        /// <summary>
        /// The world in which this object exists.
        /// </summary>
        public string WorldName { get; init; }

        /// <summary>
        /// Initializes a new instance of <see cref="GameObject"/>.
        /// </summary>
        /// <param name="eventApplier">Delegate used to perform a double-dispatch invocation to inform the <see cref="HuntBotGame"/> instance of changes.</param>
        public GameObject(Action<object> eventApplier) : base(eventApplier) { }

        /// <summary>
        /// Matches an event to the event type and applies the corresponding changes to the <see cref="GameObject"/> instance.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}
