using System.Collections.Generic;
using System.Linq;

namespace Molten.Core.Wpf.DragDrop
{
    /// <summary>
    /// Creates and manages all <see cref="DragDropper" /> instances.
    /// </summary>
    public class DragDropManager
    {
        private static DragDropManager _instances;
        /// <summary>
        /// Index into this property to retrieve or create DragDropper instances.
        /// </summary>
        public static DragDropManager Instances
        {
            get
            {
                if (_instances == null)
                {
                    _instances = new DragDropManager();
                }
                return _instances;
            }
        }

        /// <summary>
        /// Private constructor (this class is a singleton).
        /// </summary>
        private DragDropManager() { }

        /// <summary>
        /// Stores all of the DragDropper instances.
        /// </summary>
        private static List<DragDropper> dragDroppers = new List<DragDropper>();

        /// <summary>
        /// Gets or creates a DragDropper instance.
        /// </summary>
        /// <param name="name">The unique name of the DragDropper.</param>
        /// <returns>A <see cref="DragDropper" /> instance.</returns>
        public DragDropper this[string name]
        {
            get
            {
                var existing = dragDroppers.Where(d => d.Name == name).FirstOrDefault();
                if (existing != null)
                {
                    return existing;
                }

                var dd = new DragDropper(name);
                dragDroppers.Add(dd);
                return dd;
            }
        }
    }
}
