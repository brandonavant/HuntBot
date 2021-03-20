using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Base class for aggregate roots.
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// The list of events to be sent to the event store.
        /// </summary>
        private readonly List<object> _changes;

        /// <summary>
        /// The Unique Identifier for an aggregate.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The current version of the aggregate from an event store perspective.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Initialized a new instance of the AggregateRoot class.
        /// </summary>
        protected AggregateRoot()
        {
            _changes = new List<object>();
        }

        /// <summary>
        /// Matches an event to the event type and applies the corresponding changes to the aggregate.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected abstract void When(object @event);

        /// <summary>
        /// Applies incoming changes to the aggregate instance and stages those changes to be written to the event store.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate.</param>
        protected void ApplyChange(object @event)
        {
            When(@event);
            _changes.Add(@event);
        }

        /// <summary>
        /// Retrieves the collection of changes in IEnumerable form.
        /// </summary>
        /// <returns>An IEnumerable of pending changes.</returns>
        public IEnumerable<object> GetChanges()
        {
            return _changes.AsEnumerable();
        }

        /// <summary>
        /// Purges the <see cref="_changes"/> List.
        /// </summary>
        public void ClearChanges()
        {
            _changes.Clear();
        }

        /// <summary>
        /// Loads all events for the aggregate and re-applys them in order to the Aggregate instance.
        /// Once complete, the Aggregate will reach the "current" state.
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task Load(IAsyncEnumerable<object> history)
        {
            await foreach (var @event in history)
            {
                When(@event);
                Version++;
            }
        }
    }
}