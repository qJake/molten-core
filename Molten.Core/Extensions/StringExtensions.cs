using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molten.Core.Extensions
{
    /// <summary>
    /// Contains extension methods for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Appends the specified string to this instance.
        /// </summary>
        /// <param name="s">The string to work with.</param>
        /// <param name="extra">The string to append.</param>
        public static string Append(this string s, string extra)
        {
            StringBuilder sb = new StringBuilder(s.Length + extra.Length);
            sb.Append(s).Append(extra);
            return sb.ToString();
        }

        /// <summary>
        /// Prepends the specified string to this instance.
        /// </summary>
        /// <param name="s">The string to work with.</param>
        /// <param name="extra">The string to prepend.</param>
        public static string Prepend(this string s, string extra)
        {
            StringBuilder sb = new StringBuilder(s.Length + extra.Length);
            sb.Append(extra).Append(s);
            return sb.ToString();
        }
    }
}
