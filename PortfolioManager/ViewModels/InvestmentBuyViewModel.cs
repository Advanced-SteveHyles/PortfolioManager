using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    internal class InvestmentBuyViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private const string QuantityName = "Quantity";
        private const string PurchasePriceName = "PurchasePrice";
        private const string TransactionValueName = "TransactionValue";
        private readonly Action _completeTransaction;
        private readonly int _accountInvestmentMapId;
        private decimal _quantity;
        private decimal _transactionValue;
        private decimal _purchasePrice;

        public decimal PurchasePrice
        {
            get { return _purchasePrice; }
            set
            {
                _purchasePrice = value;
                TrangulatePrices(PurchasePriceName);
            }
        }

        public decimal Quantity
        {
            get { return _quantity; }
            set { _quantity = value; TrangulatePrices(QuantityName); }
        }

        public decimal TransactionValue
        {
            get { return _transactionValue; }
            set { _transactionValue = value; TrangulatePrices(TransactionValueName); }
        }

        public decimal Charges { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        public DateTime SettlementDate { get; set; } = DateTime.Today.AddDays(7);

        public bool RecordPrice { get; set; } = true;

        public InvestmentBuyViewModel(int accountInvestmentMapId, Action completeTransaction)
        {
            _accountInvestmentMapId = accountInvestmentMapId;
            SetCommands(Save, Cancel);
            this._completeTransaction = completeTransaction;
        }

        private void TrangulatePrices(string source)
        {            
            switch (source)
            {
                case PurchasePriceName:
                    _transactionValue = _quantity * _purchasePrice;
                    break;
                case TransactionValueName:
                    _purchasePrice = _transactionValue / _quantity;
                    break;
                case QuantityName:
                    _transactionValue = _quantity * _purchasePrice;
                    break;
            }

            OnPropertyChanged(PurchasePriceName);
            OnPropertyChanged(QuantityName);
            OnPropertyChanged(TransactionValueName);
        }

        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }

        private void Save()
        {
            var buyRequest = new InvestmentBuyRequest()
            {
                InvestmentMapId = _accountInvestmentMapId,
                BuyPrice = PurchasePrice,
                Quantity = this.Quantity,
                Value = this.TransactionValue,
                Charges = this.Charges,
                PurchaseDate = this.PurchaseDate,
                SettlementDate = this.SettlementDate,
                UpdatePriceHistory = this.RecordPrice
            };

            AccountInvestmentMapModel.Buy(buyRequest);
            
            this._completeTransaction.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}