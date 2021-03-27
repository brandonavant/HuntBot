using System;

namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    /// <typeparam name="TId">The Type associated with the Id.</typeparam>
    public abstract class Entity<TId> : IInternalEventHandler
    {
        /// <summary>
        /// Delegate with which allows the parent <see cref="AggregateRoot"/> to invoke the Apply method on this <see cref="Entity{TId}"/>.
        /// </summary>
        private readonly Action<object> _eventApplier;

        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        public TId Id { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Entity{TId}"/>.
        /// </summary>
        /// <param name="eventApplier"></param>
        public Entity(Action<object> eventApplier)
        {
            _eventApplier = eventApplier;
        }

        protected abstract void When(object @event);

        /// <summary>
        /// Applies incoming changes to the <see cref="Entity{TId}"/> instance and stages those changes to be written to the event store.
        /// </summary>
        /// <param name="event">The event to apply to the <see cref="Entity{TId}"/> instance.</param>
        protected void ApplyChange(object @event)
        {
            When(@event);
            _eventApplier(@event);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}