using System.Collections.Generic;
using System.Windows.Controls;
using PortfolioManager.UIBuilders;

namespace PortfolioManager
{
    public class AccountTabViewModel
    {
        private string v;
        
             public List<TabItem> AccountTabs => new List<TabItem>()
        {
            { BuildPortfolioTabContent.CreateAccountTab("1")},
            { BuildPortfolioTabContent.CreateAccountTab("2")},
            { BuildPortfolioTabContent.CreateAccountTab("3")}
        };

        public AccountTabViewModel(string v)
        {
            this.v = v;
        }
    }
}