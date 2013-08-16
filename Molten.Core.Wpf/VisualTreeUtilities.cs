using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Molten.Core.Wpf
{
    /// <summary>
    /// Contains helper methods to assist in navigating through the WPF visual tree.
    /// </summary>
    public static class VisualTreeUtilities
    {
        /// <summary>
        /// Traverses the visual tree and attempts to locate children of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of child to search for.</typeparam>
        /// <param name="parent">The root element to begin searching at.</param>
        /// <returns>A list of type <typeparamref name="T" /> containing any children of that type.</returns>
        public static List<T> FindVisualChildren<T>(DependencyObject parent) where T : class
        {
            List<T> children = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    children.Add(child as T);
                }
                children.AddRange(FindVisualChildren<T>(child).ToArray());
            }
            return children;
        }
    }
}
