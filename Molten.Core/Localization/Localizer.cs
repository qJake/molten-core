using System.Collections;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Molten.Core.Localization
{
    /// <summary>
    /// Provides an easy way to implement localization in an application using an ID-based storage structure.
    /// </summary>
    public static class Localizer
    {
        private const string LOCALIZATION_DIR = @"Localization\";
        private const string LOCALIZATION_EXT = ".lng";
        private const string LOCALIZATION_DEFAULT = "en-US";
        private const string LOCALIZATION_REGEX = "^(.+?)=(.+)$";

        private static Hashtable localizationTable;

        /// <summary>
        /// Optional. Initializes localization by reading language files from disk.
        /// </summary>
        /// <param name="culture">The name of the culture, in "langcode-countrycode" format (e.g. en-US). Defaults to <c>CultureInfo.CurrentCulture.Name</c> if null.</param>
        public static void Initialize(string culture = null)
        {
            string lang = culture ?? CultureInfo.CurrentCulture.Name;
            string data;
            try
            {
                data = File.ReadAllText(LOCALIZATION_DIR + lang + LOCALIZATION_EXT);
            }
            catch
            {
                try
                {
                    data = File.ReadAllText(LOCALIZATION_DIR + LOCALIZATION_DEFAULT + LOCALIZATION_EXT);
                }
                catch
                {
                    data = "";
                }
            }

            Init(data);
        }

        /// <summary>
        /// Optional. Initializes the localizer with the specified stream containing localization information.
        /// </summary>
        /// <param name="localizationData">A stream of localization data in the format ID=Value (one per line).</param>
        public static void Initialize(Stream localizationData)
        {
            localizationData.Position = 0;
            var sr = new StreamReader(localizationData);
            var data = sr.ReadToEnd();
            Init(data);
        }

        /// <summary>
        /// Common initializer that accepts string data to initialize the storage table.
        /// </summary>
        /// <param name="data">The data to parse and populate the storage table with.</param>
        private static void Init(string data)
        {
            localizationTable = new Hashtable();

            var lines = data.Split('\n');

            foreach (string line in lines)
            {
                Match m = Regex.Match(line.Trim(), LOCALIZATION_REGEX, RegexOptions.Multiline | RegexOptions.Compiled);
                if (m.Success && !m.Groups[1].Value.Trim().StartsWith("#"))
                {
                    localizationTable.Add(m.Groups[1].Value.Trim(), m.Groups[2].Value.Trim());
                }
            }

        }

        /// <summary>
        /// Localizes the specified string ID. If the localizer has not been initialized, it will initialize first.
        /// </summary>
        /// <param name="id">The ID of the localized string to retrieve.</param>
        /// <returns>The localized string, or the <paramref name="id" /> if the string was not found in the file.</returns>
        public static string Localize(string id)
        {
            if (localizationTable == null)
            {
                Initialize();
            }

            if (localizationTable.Contains(id))
            {
                return localizationTable[id] as string;
            }
            else
            {
                return id;
            }
        }

        /// <summary>
        /// Shorthand for the <see cref="Localize" /> method. If the localizer has not been initialized, it will initialize first.
        /// </summary>
        /// <param name="id">The ID of the localized string to retrieve.</param>
        /// <returns>The localized string, or the <paramref name="id" /> if the string was not found in the file.</returns>
        public static string L(string id)
        {
            return Localize(id);
        }
    }
}
