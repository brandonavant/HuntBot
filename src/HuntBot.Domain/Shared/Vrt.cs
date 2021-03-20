using System;

namespace HuntBot.Domain.Shared
{
    public record Vrt
    {
        /// <summary>
        /// The original UTC.
        /// </summary>
        private readonly DateTime _utc;

        /// <summary>
        /// The DateTime value for the <see cref="Vrt"/> object.
        /// </summary>
        public DateTime Value => _utc.AddHours(-2);

        /// <summary>
        /// Implicitly converts a <see cref="Vrt"/> to its <see cref="DateTime"/> equivalent.
        /// </summary>
        /// <param name="vrt"></param>
        public static implicit operator DateTime(Vrt vrt) => vrt.Value;

        /// <summary>
        /// Implicitly converts a <see cref="DateTime"/> (in UTC) to its <see cref="Vrt"/> equivalent.
        /// </summary>
        /// <param name="utc">The UTC value to be converted./param>
        public static implicit operator Vrt(DateTime utc) => new Vrt(utc);

        /// <summary>
        /// Initializes a new instance of <see cref="Vrt"/>.
        /// </summary>
        /// <param name="utc">The UTC to be converted into VRT.</param>
        public Vrt(DateTime utc)
        {
            _utc = utc;
        }
    }
}
