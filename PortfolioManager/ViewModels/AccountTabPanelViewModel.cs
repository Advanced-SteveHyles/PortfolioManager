using System.Collections.Generic;
using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs;

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
                var x = new List<AccountInvestmentMapDto>()
                {
                    {
                        new AccountInvestmentMapDto() {AccountId = 6}
                    }
                };

                return new ObservableCollection<AccountInvestmentMapDto>(x);
            }
        }

    }
}