using System.Windows.Forms;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views;

namespace PortfolioManager.UIBuilders
{
    public class AccountInvestmentMapWrapperDto:ViewModel
    {
        object _InvestmentTransaction;
        public object InvestmentTransaction
        {
            get
            {                
                return _InvestmentTransaction;
            }
        }

        private AccountInvestmentMapDto ai;

        public AccountInvestmentMapWrapperDto(AccountInvestmentMapDto ai)
        {
            this.ai = ai;
        }

        public int AccountInvestmentMapId => this.ai.AccountInvestmentMapId;

        public ICommand BuyCommand => new RelayCommand(Buy);

        private void Buy()
        {
            int i = 1;
            _InvestmentTransaction = new TopLevelMenu();
            OnPropertyChanged("InvestmentTransaction");
        }
    }
}