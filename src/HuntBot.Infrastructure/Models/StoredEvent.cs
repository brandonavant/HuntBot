namespace HuntBot.Infrastructure.Models
{
    public class StoredEvent
    {
        /// <summary>
        /// The event's position in the stream.
        /// </summary>
        public int StreamPosition { get; }

        /// <summary>
        /// The raw bytes of the event data.
        /// </summary>
        public byte[]  Data { get; set; }

        /// <summary>
        /// The CLR type to which the data can be deserialized.
        /// </summary>
        public string ClrType { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="StoredEvent"/>.
        /// </summary>
        /// <param name="streamPosition">The event's position in the stream.</param>
        /// <param name="data">The JSON-serialized data.</param>
        /// <param name="clrType">The CLR type to which the JSON-serialized data can be deserialized.</param>
        public StoredEvent(int streamPosition, byte[] data, string clrType)
        {
            StreamPosition = streamPosition;
            Data = data;
            ClrType = clrType;
        }
    }
}