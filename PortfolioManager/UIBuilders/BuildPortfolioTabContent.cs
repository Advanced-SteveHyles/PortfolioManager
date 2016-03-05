using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Views;

namespace PortfolioManager.UIBuilders
{
    public static class BuildPortfolioTabContent
    {
        public static TabItem CreateAccountListTab(string s)
        {
            return new TabItem()
            {
                Header = $"Portfolio {s}",

                Content = PopulateAccounts(s)
            };
        }

        private static AccountTab PopulateAccounts(string s)
        {
            return new AccountTab()
            {
                DataContext = new AccountTabViewModel(s)
            };
        }

        public static TabItem CreateAccountListTab(PortfolioDto portfolioDto)
        {
            return CreateAccountListTab(portfolioDto.Name);
        }
    }
}
