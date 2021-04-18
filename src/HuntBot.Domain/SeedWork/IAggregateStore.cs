using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuntBot.Domain.SeedWork
{
    public interface IAggregateStore
    {
        /// <summary>
        /// Checks to see if a record exists in the aggregate store for the given id.
        /// </summary>
        /// <param name="aggregateId">The unique identifier of the aggregate whose existence is being checked.</param>
        /// <typeparam name="T">The type of the aggregate.</typeparam>
        /// <returns>True if the aggregate exists in the aggregate store.</returns>
         Task<bool> Exists<T>(Guid aggregateId);

         /// <summary>
         /// Saves an aggregate to the aggregate store.
         /// </summary>
         /// <param name="aggregate">The aggregate to save.</param>
         /// <typeparam name="T">The type of the aggregate.</typeparam>
         Task Save<T>(T aggregate) where T : AggregateRoot;

         /// <summary>
         /// Loads an <see cref="AggregateRoot"/> instance from the aggregate store.
         /// </summary>
         /// <param name="aggregateId">The unique identifier of the aggregate being loaded.</param>
         /// <typeparam name="T">The type of the aggregate.</typeparam>
         /// <returns>An instance of <see cref="AggregateRoot"/> loaded from the aggregate store.</returns>
         Task<T> Load<T>(Guid aggregateId) where T : AggregateRoot;

        /// <summary>
        /// Loads a collection of <see cref="AggregateRoot"/> from the aggregate store.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate.</typeparam>
        /// <returns>A collection of <see cref="AggregateRoot"/> from the aggregate store.</returns>
         Task<List<T>> LoadAll<T>() where T : AggregateRoot;
    }
}