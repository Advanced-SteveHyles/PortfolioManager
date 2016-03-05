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
        private static AccountTabs PopulateAccounts(string s)
        {
            return new AccountTabs()
            {
                DataContext = new AccountTabViewModel(s)
            };
        }

        //private static AccountTabs PopulateAccounts(string s)
        //{
        //    return new AccountTabs()
        //    {
        //        DataContext = new AccountTabViewModel(s)
        //    };
        //}

        public static TabItem CreatePortfolioTab(PortfolioDto portfolioDto)
        {
            return CreatePortfolioTab(portfolioDto.Name);
        }

        public static TabItem CreatePortfolioTab(string s)
        {
            return new TabItem()
            {
                Header = $"Portfolio {s}",
                Content = PopulateAccounts(s)
            };
        }

        public static TabItem CreateAccountTab(string s)
        {
            return new TabItem()
            {
                Header = $"Account {s}",
//                Content = PopulateAccounts(s)
            };
        }
    }
}
