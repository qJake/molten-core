using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Molten.Core.Wpf.DragDrop
{
    /// <summary>
    /// Encapsulates a single drag/drop operation between any number of drop sources and any number of drop targets.
    /// </summary>
    public class DragDropper
    {
        /// <summary>
        /// Specifies the unique name of this drag/drop operation. This is used during the drag/drop operation to detect the contents of the drop.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Occurs when the drop event is fired on a valid target. The parameters are: package, source, target.
        /// </summary>
        public event Action<DropEventArgs> Drop;

        /// <summary>
        /// Occurs when the drag event starts. The UIElement parameter is the source (to match it with the source in the DragDropper) and the object is the package to include.
        /// </summary>
        public event Action<DragStartEventArgs> DragStart;

        /// <summary>
        /// Occurs when trying to drop a source onto a target. The "AllowDrop" property in the DropEventArgs dictates if the drop will occur or be cancelled.
        /// </summary>
        public event Action<DropEventArgs> CanDrop;

        /// <summary>
        /// Occurs if the result of CanDrop is true, when a source is dragged over this target.
        /// </summary>
        public event Action DragEnter;

        /// <summary>
        /// Occurs if the result of CanDrop is true, when a source is dragged away from this target.
        /// </summary>
        public event Action DragLeave;

        private List<UIElement> Sources { get; set; }
        private List<UIElement> Targets { get; set; }
        private UIElement currentSource;
        private bool IsMouseDetecting;
        private bool IsDragging;
        private Point StartPoint;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DragDropper class.
        /// 
        /// This class may only be initialized from within this assembly. To use the DragDropper class, refer to the <see cref="DragDropManager" />
        /// to create a new instance of this class.
        /// </summary>
        /// <param name="name">The unique name of this DragDropper (to distinguish multiple drag/drop events).</param>
        internal DragDropper(string name)
        {
            Name = name;
            Sources = new List<UIElement>();
            Targets = new List<UIElement>();
        }

        #endregion

        /// <summary>
        /// Determines whether or not this DragDropper is ready to perform drag/drop actions.
        /// </summary>
        /// <returns>True if this class is ready to perform drag/drop operations, otherwise false.</returns>
        public bool IsInitialized()
        {
            return (Sources.Count > 0 && Targets.Count > 0 && !string.IsNullOrWhiteSpace(Name));
        }

        /// <summary>
        /// Adds a valid source drag target.
        /// </summary>
        /// <param name="source">The draggable source element.</param>
        public void AddSource(UIElement source)
        {
            source.PreviewMouseLeftButtonDown += Source_MouseDown;
            source.MouseMove += Source_MouseMove;
            source.PreviewMouseLeftButtonUp += Source_MouseUp;
            Sources.Add(source);
        }

        /// <summary>
        /// Removes a valid source drag target.
        /// </summary>
        /// <param name="source">The source to remove.</param>
        public void RemoveSource(UIElement source)
        {
            source.PreviewMouseLeftButtonDown -= Source_MouseDown;
            source.MouseMove -= Source_MouseMove;
            source.PreviewMouseLeftButtonUp -= Source_MouseUp;
            Sources.Remove(source);
        }

        /// <summary>
        /// Adds a valid drop target.
        /// </summary>
        /// <param name="target">The element which can accept any of the specified <see cref="Sources" /> during a drop operation.</param>
        public void AddTarget(UIElement target)
        {
            target.DragEnter += Target_DragEnter;
            target.DragLeave += Target_DragLeave;
            target.DragOver += Target_DragOver;
            target.Drop += Target_Drop;
            target.AllowDrop = true;
            Targets.Add(target);
        }

        /// <summary>
        /// Removes the specified drop target from the list of valid drop targets.
        /// </summary>
        /// <param name="target">The specified element to remove from the list of drop targets.</param>
        public void RemoveTarget(UIElement target)
        {
            target.DragEnter -= Target_DragEnter;
            target.DragLeave -= Target_DragLeave;
            target.DragOver -= Target_DragOver;
            target.Drop -= Target_Drop;
            target.AllowDrop = false;
            Targets.Remove(target);
        }

        #region Source Events

        private void Source_MouseDown(object s, MouseEventArgs e)
        {
            if (IsInitialized())
            {
                StartPoint = e.GetPosition(null);
                IsMouseDetecting = true;
            }
        }

        private void Source_MouseMove(object s, MouseEventArgs e)
        {
            if (IsInitialized())
            {
                if (!IsDragging)
                {
                    if (IsMouseDetecting)
                    {
                        // Get the current mouse position
                        Point mousePos = e.GetPosition(null);
                        Vector diff = StartPoint - mousePos;

                        if (e.LeftButton == MouseButtonState.Pressed &&
                            Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                            Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                        {
                            DragStartEventArgs ea = new DragStartEventArgs();

                            // Invoke the DragStart event on the source to allow it to set the package contents.
                            DragStart.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);

                            DataObject dragData = new DataObject(Name, ea.Package);

                            IsMouseDetecting = false;
                            IsDragging = true;
                            currentSource = s as UIElement;

                            // This blocks until the drop operation completes.
                            System.Windows.DragDrop.DoDragDrop((s as UIElement), dragData, DragDropEffects.Move | DragDropEffects.None);

                            IsDragging = false;
                            currentSource = null;
                        }
                    }
                }
            }
        }

        private void Source_MouseUp(object s, MouseEventArgs e)
        {
            if (IsInitialized())
            {
                // Handle the case where the user depresses the mouse button, moves less than SystemParameters.Minimum*DragDistance and then releases the mouse button.
                IsMouseDetecting = false;
                IsDragging = false;
                currentSource = null;
            }
        }

        #endregion

        #region Target Events

        private void Target_DragEnter(object s, DragEventArgs e)
        {
            if (IsInitialized())
            {
                if (IsDragging)
                {
                    e.Handled = true;
                    DropEventArgs ea = new DropEventArgs()
                    {
                        Package = e.Data.GetData(Name),
                        Source = currentSource
                    };

                    CanDrop.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);

                    if (ea.AllowDrop)
                    {
                        DragEnter.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke();
                    }
                }
            }
        }

        private void Target_DragOver(object s, DragEventArgs e)
        {
            if (IsInitialized())
            {
                if (IsDragging)
                {
                    e.Handled = true;

                    DropEventArgs ea = new DropEventArgs()
                    {
                        Package = e.Data.GetData(Name),
                        Source = currentSource
                    };

                    CanDrop.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);

                    if (ea.AllowDrop)
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
            }
        }

        private void Target_DragLeave(object s, DragEventArgs e)
        {
            if (IsInitialized())
            {
                if (IsDragging)
                {
                    e.Handled = true;
                    DropEventArgs ea = new DropEventArgs()
                    {
                        Package = e.Data.GetData(Name),
                        Source = currentSource
                    };

                    CanDrop.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);

                    if (ea.AllowDrop)
                    {
                        DragLeave.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke();
                    }
                }
            }
        }

        private void Target_Drop(object s, DragEventArgs e)
        {
            if (IsInitialized())
            {
                if (IsDragging)
                {
                    e.Handled = true;
                    if (e.Data.GetDataPresent(Name))
                    {
                        DropEventArgs ea = new DropEventArgs()
                        {
                            Package = e.Data.GetData(Name),
                            Source = currentSource
                        };

                        // Invoke the CanDrop event again to ask the target if we're allowed to drop
                        CanDrop.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);

                        if (ea.AllowDrop)
                        {
                            Drop.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke(ea);
                            // Also fire DragLeave since the system won't do this for us.
                            DragLeave.GetInvocationList().Where(d => d.Target == s).First().DynamicInvoke();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
