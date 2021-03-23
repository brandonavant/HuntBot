namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    /// <typeparam name="TId">The Type associated with the Id.</typeparam>
    public abstract class Entity<TId> : IInternalEventHandler
    {
        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        public TId Id { get; protected set; }

        public void Handle(object @event)
        {
            throw new System.NotImplementedException();
        }
    }
}