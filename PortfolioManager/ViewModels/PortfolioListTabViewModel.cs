using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager
{
    public class PortfolioListTabViewModel
    {
        private AccountTabViewModel _accountTabViewModel;
        
        public List<TabItem> PortfolioTabs => new List<TabItem>()
        {
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.PortfolioList[0])},
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.PortfolioList[1])},
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.PortfolioList[2])}
        };

    }
}