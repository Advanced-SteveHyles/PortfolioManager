using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;
using PortfolioManager.ViewModels.Menus;

namespace PortfolioManager.UIBuilders
{
    public class AccountTabPanelViewModel
    {
        private readonly AccountDto _account;
        private AccountInvestmentDetailsViewModel _accountInvestmentDetailsVm;

        public int AccountId => _account.AccountId;
        public string Name => _account.Name;
        public int PortfolioId => _account.PortfolioId;
        public decimal Cash => _account.Cash;
        public decimal Valuation => _account.Valuation;
        public string Type => _account.Type;
        public decimal  AccountBalance => _account.AccountBalance;
         
        
        public AccountTabPanelViewModel(int accountId)
        {
            _account = AccountModel.GetAccount(accountId);
        }


        public AccountInvestmentDetailsViewModel AccountInvestmentDetailsVm
        {
            get
            {
                if (_accountInvestmentDetailsVm == null)
                    _accountInvestmentDetailsVm = new AccountInvestmentDetailsViewModel(_account.AccountId);
                return _accountInvestmentDetailsVm;
            }
        }
        
        public ObservableCollection<CashTransactionDto> AccountTransactions
        {
            get
            {
                var accountTransactions = AccountModel.GetAccountTransactions(_account.AccountId);
                return new ObservableCollection<CashTransactionDto>(accountTransactions);
            }
        }

    }

    public class AccountInvestmentMapWrapperDto
    {
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
        }
    }
}