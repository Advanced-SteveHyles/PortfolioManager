using System;
using System.Windows.Input;

namespace PortfolioManager.Other
{
    public class RelayCommand : ICommand
    {
        private readonly Action _command;
        private readonly Predicate<bool> _canExecute;

        public RelayCommand(Action command, Predicate<bool> canExecute)
        {
            _canExecute = canExecute;
            _command = command;
        }

        public RelayCommand(Action command)
        {
            this._command = command;
            _canExecute = new Predicate<bool>(x => true);
        }

        public bool CanExecute(object parameter)
        {
            return parameter == null 
                ? _canExecute.Invoke(true) 
                : _canExecute.Invoke((bool)parameter);
        }

        public void Execute(object parameter)
        {
            _command.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}