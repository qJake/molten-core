using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Molten.Core.Wpf.Commands
{
    /// <summary>
    /// Implements the ICommand interface asynchronously. While the operation is executing, the Command's CanExecute method returns false.
    /// </summary>
    public class AsyncCommand : ICommand
    {
        /// <summary>
        /// Stores the execution action.
        /// </summary>
        protected readonly Func<Task> execute;

        /// <summary>
        /// Stores the CanExecute function.
        /// </summary>
        protected readonly Func<bool> canExecute;

        /// <summary>
        /// Stores if the async action is being executed.
        /// </summary>
        protected bool isExecuting;

        /// <summary>
        /// Initializes a new instance of the AsyncCommand class with the specified asynchronous operation as the command target.
        /// </summary>
        /// <param name="execute">The async method to execute as the command target.</param>
        public AsyncCommand(Func<Task> execute) : this(execute, () => true) { }

        /// <summary>
        /// Initializes a new instance of the AsyncCommand class with the specified asynchronous operation as the command target
        /// and specified method to determine if the command is able to be executed.
        /// </summary>
        /// <param name="execute">The async method to execute as the command target.</param>
        /// <param name="canExecute">The method to use to check if this command may be executed. This should not include a check for if this command is currently executing, which is handled internally.</param>
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Returns whether or not this command is currently able to be executed.
        /// </summary>
        /// <param name="parameter">The parameter to pass in. [Not used.]</param>
        /// <returns>True if the command can currently be executed or is already being executed asynchronously, false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return !isExecuting && canExecute();
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
        /// Executes the async method stored within this AsyncCommand object.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the execution method. [Not used.]</param>
        public async void Execute(object parameter)
        {
            isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                await execute();
            }
            finally
            {
                isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Invokes the CanExecuteChanged event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
