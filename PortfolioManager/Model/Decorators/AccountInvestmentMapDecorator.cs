using System.Windows.Controls;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Other;
using PortfolioManager.ViewModels;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.UIBuilders
{
    public class AccountInvestmentMapDecorator:ViewModel
    {
        public int AccountInvestmentMapId => this._accountInvestmentMapDto.AccountInvestmentMapId;
        public string InvestmentName => this._accountInvestmentMapDto.InvestmentName;
        private const string InvestmentTransactionName = "InvestmentTransaction";

        public decimal Quantity => this._accountInvestmentMapDto.Quantity ;
        public decimal Valuation => this._accountInvestmentMapDto.Valuation;

        private UserControl _investmentTransaction;
        public UserControl InvestmentTransaction => _investmentTransaction;

        private readonly AccountInvestmentMapDto _accountInvestmentMapDto;

        public AccountInvestmentMapDecorator(AccountInvestmentMapDto accountInvestmentMapDto)
        {
            this._accountInvestmentMapDto = accountInvestmentMapDto;
        }
        
        public ICommand BuyCommand => new RelayCommand(Buy);
        public ICommand SellCommand => new RelayCommand(Sell);
        public ICommand LoyaltyCommand => new RelayCommand(Loyalty);
        public ICommand DividendCommand => new RelayCommand(Dividend);


        private void Buy()
        {
            _investmentTransaction = new InvestmentBuyView()
            {
                DataContext = new InvestmentBuyViewModel(AccountInvestmentMapId, CompleteTransaction)
            };            
            OnPropertyChanged(InvestmentTransactionName);
        }

        private void Loyalty()
        {
            _investmentTransaction = new InvestmentLoyaltyBonusView()
            {
                DataContext = new InvestmentLoyaltyBonusViewModel(AccountInvestmentMapId, CompleteTransaction)
            };
            OnPropertyChanged(InvestmentTransactionName);
        }



        private void Sell()
        {
            _investmentTransaction = new InvestmentSellView()
            {
                DataContext = new InvestmentSellViewModel(AccountInvestmentMapId,CompleteTransaction)
            };
            OnPropertyChanged(InvestmentTransactionName);
        }

        
            private void Dividend()
        {
            _investmentTransaction = new InvestmentDividendView()
            {
                DataContext = new InvestmentDividendViewModel(AccountInvestmentMapId, CompleteTransaction)
            };
            OnPropertyChanged(InvestmentTransactionName);
        }


        private void CompleteTransaction()
        {
            _investmentTransaction = null;
            OnPropertyChanged(InvestmentTransactionName);
        }

    }
}