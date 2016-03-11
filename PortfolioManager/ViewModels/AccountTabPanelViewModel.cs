using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;

namespace PortfolioManager.UIBuilders
{
    public class AccountTabPanelViewModel
    {
        private int accountId;

        public AccountTabPanelViewModel(int accountId)
        {
            this.accountId = accountId;
        }

        public ObservableCollection<AccountInvestmentMapDto> InvestmentMaps
        {
            get
            {
                var accountInvestmentMaps = AccountInvestmentMapModel.GetInvestments(accountId);
                return new ObservableCollection<AccountInvestmentMapDto>(accountInvestmentMaps);
            }
        }

        public ObservableCollection<CashTransactionDto> AccountTransactions
        {
            get
            {
                var accountTransactions = AccountModel.GetAccountTransactions(accountId);
                return new ObservableCollection<CashTransactionDto>(accountTransactions);
            }
        }

    }
}