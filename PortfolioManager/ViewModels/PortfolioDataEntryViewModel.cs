using System;
using System.Windows.Input;

namespace PortfolioManager.ViewModels.Menus
{
    public class PortfolioDataEntryViewModel : AbstractSaveCancelCommands
    {
        private readonly Action dialogClose;

        
        public PortfolioDataEntryViewModel(Action dialogClose): base()
        {
            this.dialogClose = dialogClose;
            SetCommands(Save, Cancel);            
        }

        public string PortfolioName { get; set; }

        private void Save()
        {
            throw new NotImplementedException($"{PortfolioName} cannot be saved");
        }

        private void Cancel()
        {
            this.dialogClose.Invoke();
        }
    }
}