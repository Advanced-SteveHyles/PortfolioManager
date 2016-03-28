using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class CashWithdrawalViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private readonly int _accountId;
        private readonly Action _completeTransaction;

        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal TransactionValue { get; set; }
        public string TransactionSource { get; set; }

        public ObservableCollection<string> TransactionTypes => new ObservableCollection<string>(CashWithdrawalTransactionTypes.WithdrawalTypes);

        public string SelectedTransactionType { get; set; }

        public CashWithdrawalViewModel(int accountId, Action completeTransaction)
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
            var request = new WithdrawalTransactionRequest()
            {
                AccountId = this._accountId,
                Source = this.TransactionSource,
                TransactionDate = this.TransactionDate,
                Value = this.TransactionValue,
                TransactionType = SelectedTransactionType
            };

            AccountTransactionModel.InsertWithdrawal(request);

            this._completeTransaction.Invoke();
        }

        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }
    }
}