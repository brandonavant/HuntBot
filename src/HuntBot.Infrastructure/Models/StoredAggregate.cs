using System;
using System.Collections.Generic;

namespace HuntBot.Infrastructure.Models
{
    public class StoredAggregate
    {
        /// <summary>
        /// The aggregate id.
        /// </summary>
        /// <value></value>
        public Guid Id { get; }
        
        /// <summary>
        /// Encapsulates the collection of stored events for this aggregate.
        /// </summary>
        /// <value></value>
        public List<StoredEvent> StoredEvents { get; }

        /// <summary>
        /// Initializes a new isntance of <see cref="StoredAggregate"/>.
        /// </summary>
        /// <param name="id">The aggregate id.</param>
        /// <param name="storedEvents">Encapsulates the collection of stored events for this aggregate.</param>
        public StoredAggregate(Guid id, List<StoredEvent> storedEvents)
        {
            Id = id;
            StoredEvents = storedEvents;
        }
    }
}