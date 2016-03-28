using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Views.TabPanels;

namespace PortfolioManager.UIBuilders
{
    public static class BuildPortfolioTabContent
    {


        //private static AccountTabs PopulateAccounts(string s)
        //{
        //    return new AccountTabs()
        //    {
        //        DataContext = new AccountTabViewModel(s)
        //    };
        //}

        //public static TabItem CreatePortfolioTabItem(PortfolioDto portfolioDto)
        public static TabItem CreatePortfolioTabItem(PortfolioDto portfolioDto)
        {
            var portfolioTabItem = new TabItem()
            {
                Header = $"{portfolioDto.Name}",
                Content = new PortfolioTabPanel { DataContext = new PortfolioTabPanelViewModel(portfolioDto.PortfolioId) } //PopulateAccounts(s)
            };
                        
         return portfolioTabItem;            
        }

        public static TabItem CreateAccountTabItem(AccountDto account)
        {
            return new TabItem()
            {
                Header = $"{account.Name}",
                Content = new AccountTabPanel() {DataContext = new AccountTabPanelViewModel(account.AccountId) }
            };
        }
    }
}
