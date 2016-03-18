﻿using System.Windows.Controls;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.UIBuilders
{
    public class AccountInvestmentMapDecorator:ViewModel
    {
        public int AccountInvestmentMapId => this._accountInvestmentMapDto.AccountInvestmentMapId;
        public string InvestmentName => this._accountInvestmentMapDto.InvestmentName;

        private UserControl _investmentTransaction;
        public UserControl InvestmentTransaction => _investmentTransaction;

        private readonly AccountInvestmentMapDto _accountInvestmentMapDto;

        public AccountInvestmentMapDecorator(AccountInvestmentMapDto accountInvestmentMapDto)
        {
            this._accountInvestmentMapDto = accountInvestmentMapDto;
        }
        
        public ICommand BuyCommand => new RelayCommand(Buy);
        public ICommand SellCommand => new RelayCommand(Sell);


        private void Buy()
        {
            _investmentTransaction = new InvestmentBuyView()
            {
                DataContext = new InvestmentBuyViewModel(AccountInvestmentMapId, CompleteTransaction)
            };
            OnPropertyChanged("InvestmentTransaction");
        }


        private void Sell()
        {
            _investmentTransaction = new InvestmentSellView()
            {
                DataContext = new InvestmentSellViewModel(CompleteTransaction)
            };
            OnPropertyChanged("InvestmentTransaction");
        }

     
        private void CompleteTransaction()
        {
            _investmentTransaction = null;
            OnPropertyChanged("InvestmentTransaction");
        }

    }
}