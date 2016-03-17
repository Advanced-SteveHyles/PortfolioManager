using System.Collections.ObjectModel;
using System.Linq;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Model;

namespace PortfolioManager.UIBuilders
{
    public class AccountInvestmentDetailsViewModel
    {



        private readonly int _accountId;

        public AccountInvestmentDetailsViewModel(int accountId)
        {
            _accountId = accountId;            
        }

        public ObservableCollection<AccountInvestmentMapDto> InvestmentMaps
        {
            get
            {
                var accountInvestmentMaps = AccountInvestmentMapModel.GetInvestments(_accountId);
                return new ObservableCollection<AccountInvestmentMapDto>(accountInvestmentMaps);
            }
        }

        public ObservableCollection<AccountInvestmentMapWrapperDto> InvestmentMapWrappers
        {
            get
            {
                var accountInvestmentMaps = AccountInvestmentMapModel.GetInvestments(_accountId);
                var wrappers = accountInvestmentMaps.Select(ai => new AccountInvestmentMapWrapperDto(ai));

                return new ObservableCollection<AccountInvestmentMapWrapperDto>(wrappers);
            }
        }
        
    }
}