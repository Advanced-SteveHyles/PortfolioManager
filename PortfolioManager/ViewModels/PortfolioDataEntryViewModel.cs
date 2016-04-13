using System;
using Portfolio.Common.DTO.Requests;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
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
            var portfolioRequest = new PortfolioRequest {Name = PortfolioName};
            PortfolioModel.InsertPortfolio(portfolioRequest);

            this.dialogClose.Invoke();
        }

        private void Cancel()
        {
            this.dialogClose.Invoke();
        }
    }
}