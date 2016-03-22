using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    internal class InvestmentLoyaltyBonusViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private readonly Action _completeTransaction;
        private readonly int _accountInvestmentMapId;

        public decimal TransactionValue { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Today;

        public InvestmentLoyaltyBonusViewModel(int accountInvestmentMapId, Action completeTransaction)
        {
            _accountInvestmentMapId = accountInvestmentMapId;
            SetCommands(Save, Cancel);
            this._completeTransaction = completeTransaction;
        }
        
        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }

        private void Save()
        {
            var loyaltyBonusRequest = new  InvestmentLoyaltyBonusRequest()
            {
                InvestmentMapId = _accountInvestmentMapId,                
                Amount = this.TransactionValue,                
                TransactionDate = this.TransactionDate,                
            };

            AccountInvestmentMapModel.ApplyLoyaltyBonus(loyaltyBonusRequest);

            this._completeTransaction.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}