using System.Windows;

namespace Molten.Core.Wpf.DragDrop
{
    /// <summary>
    /// Stores information related to a Drop event.
    /// </summary>
    public class DropEventArgs
    {
        /// <summary>
        /// Gets or sets whether or not this drop action should be allowed.
        /// </summary>
        public bool AllowDrop { get; set; }

        /// <summary>
        /// Gets or sets the source <seealso cref="UIElement" /> that this drop originated from.
        /// </summary>
        public UIElement Source { get; set; }

        /// <summary>
        /// Gets or sets the package contents of this drop.
        /// </summary>
        public object Package { get; set; }
    }
}
