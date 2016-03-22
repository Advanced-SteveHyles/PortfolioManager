using System.Collections.ObjectModel;
using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
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

        public ObservableCollection<AccountInvestmentMapDecorator> InvestmentMapWrappers
        {
            get
            {
                var accountInvestmentMaps = AccountInvestmentMapModel.GetInvestments(_accountId);
                var wrappers = accountInvestmentMaps.Select(ai => new AccountInvestmentMapDecorator(ai));

                return new ObservableCollection<AccountInvestmentMapDecorator>(wrappers);
            }
        }
        
    }
}