using System;
using System.Windows.Input;

namespace Molten.Core.Wpf.Commands
{
    /// <summary>
    /// Implements the ICommand interface using an <see cref="Action" /> delegate.
    /// </summary>
    public class ActionCommand : ICommand
    {
        /// <summary>
        /// Stores the action to be executed.
        /// </summary>
        protected Action execute;

        /// <summary>
        /// Stores the method to check if we can execute the action.
        /// </summary>
        protected Func<bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the ActionCommand class with the specified action to execute.
        /// </summary>
        /// <param name="executeAction">The action to perform when the command is executed.</param>
        public ActionCommand(Action executeAction) : this(executeAction, () => true) { }

        /// <summary>
        /// Initializes a new instance of the ActionCommand class with the specified action to execute, 
        /// and the specified method to use to check if the command can be executed.
        /// </summary>
        /// <param name="executeAction">The action to perform when the command is executed.</param>
        /// <param name="canExecuteMethod">The method to use to check if this command can be executed.</param>
        public ActionCommand(Action executeAction, Func<bool> canExecuteMethod)
        {
            execute = executeAction;
            canExecute = canExecuteMethod;
        }

        /// <summary>
        /// Returns whether or not this command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the method. [Not used.]</param>
        /// <returns>True if the command can be executed currently, false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        /// <summary>
        /// Executes the action contained within this ActionCommand object.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the action. [Not used.]</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute();
            }
        }

        /// <summary>
        /// Occurs when the CanExecute state has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invalidates the execution state and forces the CanExecute function to re-evaluate.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
