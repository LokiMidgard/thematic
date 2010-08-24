using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ThemeMatic.Components
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<bool> canExecute;

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public DelegateCommand(Action<object> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute.Invoke();                
            }
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute.Invoke(parameter);
        }
    }
}
