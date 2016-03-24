using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class FeesViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private int _accountId;
        private readonly Action _completeTransaction;

        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal TransactionValue { get; set; }
        public string TransactionSource { get; set; }

        public FeesViewModel(int accountId, Action completeTransaction)
        {
            _accountId = accountId;
            SetCommands(Save, Cancel);
            this._completeTransaction = completeTransaction;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Save()
        {
            var request = new FeeTransactionRequest()
            {
                AccountId = this._accountId,                
                TransactionDate = this.TransactionDate,
                Value = this.TransactionValue
            };

            AccountTransactionModel.InsertFee(request);

            this._completeTransaction.Invoke();
        }

        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }
    }
}