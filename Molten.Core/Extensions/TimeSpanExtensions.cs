using System;
using System.Collections.Generic;

namespace Molten.Core.Extensions
{
    /// <summary>
    /// Contains extension methods for the TimeSpan class.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Specifies the absolute maximum number of parts to include when abbreviating a TimeSpan.
        /// </summary>
        private const int MAX_ABBREVIATION_PARTS = 4;

        /// <summary>
        /// Converts this TimeSpan into an abbreviated format.
        /// </summary>
        /// <returns>An abbreviated string describing this TimeSpan (e.g. 2d 3h 4m 56s).</returns>
        public static string ToAbbreviatedString(this TimeSpan dt)
        {
            // Run with the maximum number of parts if none specified.
            return ToAbbreviatedString(dt, MAX_ABBREVIATION_PARTS);
        }

        /// <summary>
        /// Converts this TimeSpan into an abbreviated format, specifying the number of parts to use in the output.
        /// </summary>
        /// <param name="dt">This TimeSpan to convert.</param>
        /// <param name="numberOfParts">The number of time components to include in the output.</param>
        /// <returns>An abbreviated string describing this TimeSpan (e.g. 2d 3h 4m 56s).</returns>
        /// <remarks>TODO: Support months (maybe, this is difficult due to 30/31 days), years, and milliseconds.</remarks>
        public static string ToAbbreviatedString(this TimeSpan dt, int numberOfParts)
        {
            if (numberOfParts > MAX_ABBREVIATION_PARTS || numberOfParts <= 0)
            {
                throw new ArgumentException("numberOfParts must be between 1 and " + MAX_ABBREVIATION_PARTS + ".", "numberOfParts");
            }

            List<string> pieces = new List<string>();

            if (dt.Days > 0)
            {
                pieces.Add(dt.Days + "d");
            }
            if (dt.Hours > 0)
            {
                pieces.Add(dt.Hours + "h");
            }
            if (dt.Minutes > 0)
            {
                pieces.Add(dt.Minutes + "m");
            }
            if (dt.Seconds > 0)
            {
                pieces.Add(dt.Seconds + "s");
            }

            if (numberOfParts > pieces.Count)
            {
                numberOfParts = pieces.Count;
            }

            // How many parts do we want?
            pieces.RemoveRange(numberOfParts, (pieces.Count - numberOfParts));

            return string.Join(" ", pieces.ToArray());
        }
    }
}
