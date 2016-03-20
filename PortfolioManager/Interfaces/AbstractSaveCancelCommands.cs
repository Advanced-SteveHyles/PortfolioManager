using System;
using System.Windows.Input;
using PortfolioManager.Other;
using PortfolioManager.ViewModels.Menus;

namespace PortfolioManager.Interfaces
{
    public abstract class AbstractSaveCancelCommands
    {
        private Action _saveCancelCommand;
        private Action _cancelCommand;

        protected void SetCommands(Action saveCancelCommand, Action cancelCommand)
        {
            _cancelCommand = cancelCommand;
            _saveCancelCommand = saveCancelCommand;
        }

        public ICommand SaveCommand => new RelayCommand(_saveCancelCommand);

        public ICommand CancelCommand => new RelayCommand(_cancelCommand);
    }
}