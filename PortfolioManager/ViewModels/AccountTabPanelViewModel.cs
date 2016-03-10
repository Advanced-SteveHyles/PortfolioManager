using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs;
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

    }
}