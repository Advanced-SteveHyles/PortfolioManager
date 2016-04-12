using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class AccountTransactionSummaryViewModel
    {
        private readonly int _accountId;
        
        public AccountTransactionSummaryViewModel(int accountId)
        {
            _accountId = accountId;            
        }

        public ObservableCollection<CashTransactionDto> AccountTransactions
        {
            get
            {
                var accountTransactions = AccountModel.GetAccountTransactions(_accountId);
                return new ObservableCollection<CashTransactionDto>(accountTransactions);
            }
        }
    }
}