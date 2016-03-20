using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class CashDepositViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private int _accountId;
        private readonly Action _completeTransaction;

        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal TransactionValue { get; set; }
        public string TransactionSource { get; set; }
        public bool IsTaxRefund { get; set; } = false;

        public CashDepositViewModel(int accountId, Action completeTransaction)
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
            var request = new DepositTransactionRequest()
            {
                AccountId = this._accountId,
                IsTaxRefund = this.IsTaxRefund,
                Source = this.TransactionSource,
                TransactionDate = this.TransactionDate,
                Value = this.TransactionValue
            };

            AccountTransactionModel.InsertDeposit(request);
                       
            this._completeTransaction.Invoke();
        }

        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }
    }
}