using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Molten.Core.Wpf.Converters
{
    /// <summary>
    /// A type converter for visibility and boolean values.
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a hexadecimal color string to a Color object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter, or null if no parameter was defined.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                return null;
            }
            if (targetType.IsAssignableFrom(typeof (Color)))
            {
                return (Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value);
            }
            if (targetType.IsAssignableFrom(typeof(Brush)))
            {
                return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString((string)value));
            }
            return null;
        }

        /// <summary>
        /// Converts a Visibility value to a boolean value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter, or null if no parameter was defined.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>Converting back is not supported at this time.</remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
