using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Molten.Core.Wpf.Converters
{
    /// <summary>
    /// A type converter for inversing a boolean value.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter, or null if no parameter was defined.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            throw new ArgumentException("The parameter passed in to InverseBooleanConverter is not of type bool.", "value"); 
        }

        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter, or null if no parameter was defined.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            throw new ArgumentException("The parameter passed in to InverseBooleanConverter is not of type bool.", "value");
        }
    }
}
