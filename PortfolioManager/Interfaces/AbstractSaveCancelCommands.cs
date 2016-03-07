using System;
using System.Windows.Input;

namespace PortfolioManager.ViewModels.Menus
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

   public ICommand SaveCommand
        {
            get { return new RelayCommand(_saveCancelCommand); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(_cancelCommand); }
        }
    }
}