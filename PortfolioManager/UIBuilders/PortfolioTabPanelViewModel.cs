using System.Collections.Generic;
using System.Windows.Controls;
using PortfolioManager.Views;

namespace PortfolioManager.UIBuilders
{
    public class PortfolioTabPanelViewModel
    {
        private readonly string _portfolioOfInterest;

        public List<TabItem> AccountTabs => new List<TabItem>()
        {
            { BuildPortfolioTabContent.CreateAccountTabItem("Account 1")},
            { BuildPortfolioTabContent.CreateAccountTabItem("Account 2")},
            { BuildPortfolioTabContent.CreateAccountTabItem("Account 3")}
        };


        public PortfolioTabPanelViewModel(string portfolioOfInterest)
        {
            _portfolioOfInterest = portfolioOfInterest;
        }

        private static AccountTabs PopulateAccounts(string s)
        {
            return new AccountTabs()
            {
                DataContext = new AccountTabViewModel(s)
            };
        }
    }
}