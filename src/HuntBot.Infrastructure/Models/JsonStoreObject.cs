using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntBot.Infrastructure.Models
{
    /// <summary>
    /// Represents a JSON-serialized object stored in the database.
    /// </summary>
    public class JsonStoreObject
    {
        /// <summary>
        /// The unique key for the stored object.
        /// </summary>
        public string Key { get; init; }

        /// <summary>
        /// The raw bytes of the stored object.
        /// </summary>
        public byte[] Value { get; init; }
    }
}
