namespace HuntBot.Domain.SeedWork
{
    public interface IInternalEventHandler
    {
        /// <summary>
        /// Provides an <see cref="AggregateRoot"/> class with the ability
        /// to signal an <see cref="Entity{TId}"/> to handle its own events.
        /// </summary>
        /// <param name="event">The event to handle./></param>
        public void Handle(object @event);
    }
}
