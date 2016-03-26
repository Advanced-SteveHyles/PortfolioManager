using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    internal class InvestmentSellViewModel : AbstractSaveCancelCommands, INotifyPropertyChanged
    {
        private const string QuantityName = "Quantity";
        private const string SellingPriceName = "SellingPrice";
        private const string TransactionValueName = "TransactionValue";
        private readonly Action _completeTransaction;
        private readonly int _accountInvestmentMapId;
        private decimal _quantity;
        private decimal _transactionValue;
        private decimal _sellingPrice;

        public decimal SellingPrice
        {
            get { return _sellingPrice; }
            set
            {
                _sellingPrice = value;
                TrangulatePrices(SellingPriceName);
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

        public DateTime SaleDate { get; set; } = DateTime.Today;

        public DateTime SettlementDate { get; set; } = DateTime.Today.AddDays(7);

        public bool RecordPrice { get; set; } = true;

        public InvestmentSellViewModel(int accountInvestmentMapId, Action completeTransaction)
        {
            _accountInvestmentMapId = accountInvestmentMapId;
            SetCommands(Save, Cancel);
            this._completeTransaction = completeTransaction;
        }

        private void TrangulatePrices(string source)
        {
            switch (source)
            {
                case SellingPriceName:
                    _transactionValue = _quantity * _sellingPrice;
                    break;
                case TransactionValueName:
                    _sellingPrice = _transactionValue / _quantity;
                    break;
                case QuantityName:
                    _transactionValue = _quantity * _sellingPrice;
                    break;
            }

            OnPropertyChanged(SellingPriceName);
            OnPropertyChanged(QuantityName);
            OnPropertyChanged(TransactionValueName);
        }

        private void Cancel()
        {
            this._completeTransaction.Invoke();
        }

        private void Save()
        {
            var sellRequest = new InvestmentSellRequest()
            {
                InvestmentMapId = _accountInvestmentMapId,
                SellPrice = SellingPrice,
                Quantity = this.Quantity,
                Value = this.TransactionValue,
                Charges = this.Charges,
                SellDate = this.SaleDate,
                SettlementDate = this.SettlementDate,
                UpdatePriceHistory = this.RecordPrice
            };

            AccountInvestmentMapModel.Sell(sellRequest);

            this._completeTransaction.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}