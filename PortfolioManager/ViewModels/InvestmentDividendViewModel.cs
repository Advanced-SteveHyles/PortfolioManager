using System;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    internal class InvestmentDividendViewModel : AbstractSaveCancelCommands
    {
        private readonly int _accountInvestmentMapId;
        private readonly Action completeTransaction;
        public decimal TransactionValue { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        public InvestmentDividendViewModel(int accountInvestmentMapId, Action completeTransaction)
        {
            _accountInvestmentMapId = accountInvestmentMapId;
            SetCommands(Save, Cancel);
            this.completeTransaction = completeTransaction;
        }

        private void Cancel()
        {
            this.completeTransaction.Invoke();
        }

        private void Save()
        {
            var investmentDividendRequest = new InvestmentDividendRequest()
            {
                InvestmentMapId = _accountInvestmentMapId,
                Amount = this.TransactionValue,
                TransactionDate = this.TransactionDate,
            };

            AccountInvestmentMapModel.ApplyDividend(investmentDividendRequest);

            this.completeTransaction.Invoke();
        }


    }
}