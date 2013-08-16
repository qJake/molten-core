using System.Windows;
using System.Windows.Data;

namespace Molten.Core.Wpf
{
    /// <summary>
    /// Contains methods that assist in getting and setting dependency property values on dependency objects based on a property/binding path
    /// by using a dummy dependency object to store and retrieve values.
    /// </summary>
    public static class PropertyPathHelper
    {
        /// <summary>
        /// Retrieves the value of the dependency property resolved from the <paramref name="propertyPath" /> on the object <paramref name="obj" />.
        /// </summary>
        /// <param name="obj">The object to search in.</param>
        /// <param name="propertyPath">The path of the property to retrieve the value for. Supports any syntax that {Binding Path=...} does.</param>
        /// <returns>The value of the dependency property.</returns>
        public static object GetValue(object obj, string propertyPath)
        {
            Binding binding = new Binding(propertyPath);
            binding.Mode = BindingMode.OneTime;
            binding.Source = obj;
            BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
            return _dummy.GetValue(Dummy.ValueProperty);
        }

        /// <summary>
        /// Sets the value of the dependency property resolved from <paramref name="propertyPath" /> within <paramref name="obj" /> to <paramref name="value" />.
        /// </summary>
        /// <param name="obj">The object to search in.</param>
        /// <param name="propertyPath">The path of the property to retrieve the value for. Supports any syntax that {Binding Path=...} does.</param>
        /// <param name="value">The value to set the dependency property to.</param>
        public static void SetValue(object obj, string propertyPath, object value)
        {
            Binding binding = new Binding(propertyPath);
            binding.Mode = BindingMode.TwoWay;
            binding.Source = obj;
            BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
            _dummy.SetValue(Dummy.ValueProperty, value);
        }

        private static readonly Dummy _dummy = new Dummy();

        private class Dummy : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(Dummy), new UIPropertyMetadata(null));
        }
    }
}
