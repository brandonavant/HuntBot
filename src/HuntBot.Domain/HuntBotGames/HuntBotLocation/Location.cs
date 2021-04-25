using HuntBot.Domain.Constants;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HuntBot.Domain.HuntBotGames.HuntBotLocation
{
    public record Location
    {
        /// <summary>
        /// The HuntBot instance's position (in centimeters) along the east/west axis, with west being positive.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The HuntBot instance's altitude (in centimeters) above the ground, with negative being below ground.
        /// </summary>
        public float Y { get; set; } = 0;

        /// <summary>
        /// The HuntBot instance's position (in centimeters) along the north/south axis, with north being positive.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The HuntBot instance's yaw, in tenth's of a degree (0 to 3599).
        /// </summary>
        public float Yaw { get; set; } = 0;

        /// <summary>
        /// Enumeration for pulling a specified group from the Location regex match's groups collection.
        /// </summary>
        private enum LocationRegexGroupIndex
        {
            Capture = 0,
            NorthSouthPos = 1,
            EastWestPos = 2,
            AltitudePos = 3,
            Yaw = 4
        }

        /// <summary>
        /// Converts the string representation into an instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">When the method returns, the result will contain the value of the conversion, if successful. If not successful, the result will be null.</param>
        /// <returns>True if the conversion was successful; false otherwise.</returns>
        public static bool TryParseLocation(string value, out Location result)
        {
            var location = new Location();
            var regexPattern = HuntBotRegex.Coordinates;
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(value);

            result = null;

            // We require the NSEW pieces, at least.
            if (!match.Groups[(int)LocationRegexGroupIndex.NorthSouthPos].Success || !match.Groups[(int)LocationRegexGroupIndex.EastWestPos].Success)
            {
                return false;
            }

            try
            {
                var altitudePiece = match.Groups[(int)LocationRegexGroupIndex.AltitudePos];
                var nsPiece = match.Groups[(int)LocationRegexGroupIndex.NorthSouthPos];
                var ewPiece = match.Groups[(int)LocationRegexGroupIndex.EastWestPos];
                var yawPiece = match.Groups[(int)LocationRegexGroupIndex.Yaw];

                // We don't technically need to do a TryParse, because the regex has taken care of that for us.
                location.Z = float.Parse(GetValueForCoordinatePiece(nsPiece.Value));
                location.X = float.Parse(GetValueForCoordinatePiece(ewPiece.Value));
                location.Y = altitudePiece.Success ? float.Parse(altitudePiece.Value) : 0;
                location.Yaw = yawPiece.Success ? float.Parse(yawPiece.Value) : 0;

                result = location;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Strips the NSEW and A suffixes off of coordinate pieces, yielding only the numeric portion.
        /// </summary>
        /// <param name="piece">The coordinate piece from which the numbers are parsed.</param>
        /// <returns>The numeric-only portion of the input.</returns>
        private static string GetValueForCoordinatePiece(string piece)
        {
            return new string(piece.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}
