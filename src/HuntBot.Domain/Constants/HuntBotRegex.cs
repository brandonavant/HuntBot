namespace HuntBot.Domain.Constants
{
    public static class HuntBotRegex
    {
        /// <summary>
        /// Regular expression used to parse the World, X, Y, Z and Yaw tokens out of a string. 
        /// For example, given the string "AW 120.44N 140.77E 0.5A 3599", the values
        /// World = AW, X = 140.77, Y = 0.5, Z = 120.44, and Yaw = 3599 will be parsed out.
        /// </summary>
        /// TODO: This can be improved more to place limitation on decimal places and number ranges.
        public const string Coordinates = @"^(\w+){1}\s+(\d*\.?\d+[NS]){1}\s+(\d*\.?\d+[EW]){1}(\s+\d*\.?\d+[A])?(\s+\d+)?";
    }
}
