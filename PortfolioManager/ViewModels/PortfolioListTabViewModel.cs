using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager
{
    public class PortfolioListTabViewModel
    {
        private AccountTabViewModel _accountTabViewModel;
        
        public List<TabItem> PortfolioTabs => new List<TabItem>()
        {
            { BuildPortfolioTabContent.CreateAccountListTab("A")},
            { BuildPortfolioTabContent.CreateAccountListTab("B")},
            { BuildPortfolioTabContent.CreateAccountListTab("C")}
        };

    }

    public class AccountTabViewModel
    {
        private string v;

        public AccountTabViewModel(string v)
        {
            this.v = v;
        }
    }
}