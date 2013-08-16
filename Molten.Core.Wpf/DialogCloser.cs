using System;
using System.Windows;
using System.Windows.Controls;

namespace Molten.Core.Wpf
{
    /// <summary>
    /// Contains a dependency property that can be attached to a button. The DialogResult property is bindable, and when its value is set, the window closes.
    /// </summary>
    public static class DialogCloser
    {
        /// <summary>
        /// Retrieves the DialogResult property value.
        /// </summary>
        /// <param name="obj">The dependency object containing the property.</param>
        /// <returns>The value of the property.</returns>
        public static bool? GetDialogResult(DependencyObject obj) { return (bool?)obj.GetValue(DialogResultProperty); }

        /// <summary>
        /// Sets the value of the DialogResult property.
        /// </summary>
        /// <param name="obj">The dependency object containing the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDialogResult(DependencyObject obj, bool? value) { obj.SetValue(DialogResultProperty, value); }

        /// <summary>
        /// The DialogResult dependency property.
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogCloser), new UIPropertyMetadata
        {
            PropertyChangedCallback = (obj, e) =>
            {
                Button button = obj as Button;

                if (button == null)
                {
                    throw new InvalidOperationException("You can only use DialogCloser.DialogResult on a Button control.");
                }

                Window.GetWindow(button).DialogResult = GetDialogResult(button);
            }
        });
    }
}
