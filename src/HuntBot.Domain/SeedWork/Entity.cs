namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    /// <typeparam name="TId">The Type associated with the Id.</typeparam>
    public abstract class Entity<TId>
    {
        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        public TId Id { get; protected set; }
    }
}