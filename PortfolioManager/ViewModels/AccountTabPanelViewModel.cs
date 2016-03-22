using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;
using PortfolioManager.ViewModels.Menus;

namespace PortfolioManager.UIBuilders
{
    public class AccountTabPanelViewModel : ViewModel
    {
    
        private readonly AccountDto _account;
        private AccountInvestmentDetailsViewModel _accountInvestmentDetailsVm;
        private AccountDetailsViewModel _accountDetailsVm;

        

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

        public AccountDetailsViewModel AccountDetailsVm
            {
            get
            {
                if (_accountDetailsVm == null)
                    _accountDetailsVm = new AccountDetailsViewModel(_account);
                return _accountDetailsVm;
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
}