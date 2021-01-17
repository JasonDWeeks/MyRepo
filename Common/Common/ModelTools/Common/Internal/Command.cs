using System;
using System.Windows.Input;

namespace Common.ModelTools.Common.Internal
{
    class Command : ICommand
    {
        readonly Action _execute;
        readonly Action<object> _executeWithParameter;


        public Command(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public Command(Action<object> executeWithParameter)
        {
            _executeWithParameter = executeWithParameter ?? throw new ArgumentNullException(nameof(executeWithParameter));
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
            _executeWithParameter?.Invoke(parameter);
        }
    }

}
