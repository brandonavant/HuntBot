using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuntBot.Domain.SeedWork
{
    /// <summary>
    /// Base class for aggregate roots.
    /// </summary>
    public abstract class AggregateRoot : IInternalEventHandler
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
        /// Initialized a new instance of <see cref="AggregateRoot"/>.
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
        /// Applies incoming changes to the <see cref="AggregateRoot"/> instance and stages those changes to be written to the event store.
        /// </summary>
        /// <param name="event">The event to apply to the <see cref="AggregateRoot"/> instance.</param>
        protected void ApplyChange(object @event)
        {
            When(@event);
            _changes.Add(@event);
        }

        /// <summary>
        /// Retrieves the collection of changes in IEnumerable form.
        /// </summary>
        /// <returns>An IEnumerable of pending changes.</returns>
        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        /// <summary>
        /// Purges the <see cref="_changes"/> List.
        /// </summary>
        public void ClearChanges() => _changes.Clear();


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

        /// <summary>
        /// Provides aggregate validation by ensuring that the <see cref="IBusinessRule"/> is not broken.
        /// </summary>
        /// <param name="rule"><The <see cref="IBusinessRule"/> whose validity is checked./param>
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        /// <summary>
        /// Allows an <see cref="Entity{TId}"/> instance to apply its own business logic to itself while keeping
        /// the <see cref="AggregateRoot"/> instance in control of the process.
        /// </summary>
        /// <param name="entity">The <see cref="Entity{TId}"/> instance for which an event is being applied.</param>
        /// <param name="event">The Event to apply to the <see cref="Entity{TId}"/> instance.</param>
        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}