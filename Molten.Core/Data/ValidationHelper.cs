using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Molten.Core.Data
{
    /// <summary>
    /// Contains helper methods to assist in data validation with Data Annotations.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates an entire data object, and returns a list of strings containing error messages.
        /// </summary>
        /// <param name="data">The data object to validate.</param>
        /// <returns>A List of strings containing error messages, or null if no errors were found.</returns>
        public static List<string> ValidateDataObject(object data)
        {
            var props = data.GetType().GetProperties();
            var errors = new List<string>();

            foreach (var p in props)
            {
                errors.AddRange(ValidateProperty(data, p.Name) ?? new List<string>());
            }

            return (errors.Count > 0 ? errors : null);
        }

        /// <summary>
        /// Validates a specific property on an object, and returns a list of strings containing error messages.
        /// </summary>
        /// <param name="data">The data object to validate.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>A List of strings containing error messages, or null if no errors were found.</returns>
        public static List<string> ValidateProperty(object data, string propertyName)
        {
            var info = data.GetType().GetProperty(propertyName);
            object value = info.GetValue(data , null);
            var errors = (from va in info.GetCustomAttributes(true).OfType<ValidationAttribute>()
                          where !va.IsValid(value)
                          select va.FormatErrorMessage(string.Empty)).ToList();

            return (errors.Any() ? errors : null);
        }
    }
}
