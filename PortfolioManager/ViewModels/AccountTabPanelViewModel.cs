using System.Collections.ObjectModel;
using System.Data;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;

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
}