using System;
using PortfolioManager.Interfaces;

namespace PortfolioManager.UIBuilders
{
    internal class InvestmentBuyViewModel: AbstractSaveCancelCommands
    {
        private readonly Action completeTransaction;
        
        public InvestmentBuyViewModel(Action completeTransaction)
        {
            SetCommands(Save, Cancel);
            this.completeTransaction = completeTransaction;
        }

        private void Cancel()
        {
            this.completeTransaction.Invoke();            
        }

        private void Save()
        {
            this.completeTransaction.Invoke();
        }
    }
}