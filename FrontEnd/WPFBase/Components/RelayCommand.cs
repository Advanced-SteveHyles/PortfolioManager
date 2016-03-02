using System;
using System.Windows.Input;

namespace WPFBase.Components
{
    public class RelayCommand : ICommand
    {

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private readonly string _name;

        public RelayCommand(Action<object> execute)
        {
            this._canExecute = null;
            this._execute = execute;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute, string name)
        {
            this._canExecute = canExecute;
            this._execute = execute;
            this._name = name;
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
                return;
            }
            Console.WriteLine("Command Not Found " + _name);
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute != null)
            {
                return (bool)this._canExecute.Invoke(parameter);
            }

            return false;
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
