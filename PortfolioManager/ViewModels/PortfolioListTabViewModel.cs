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
        public List<TabItem> PortfolioTabs => new List<TabItem>()
        {
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.GetPortfolioList()[0])},
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.GetPortfolioList()[1])},
            { BuildPortfolioTabContent.CreatePortfolioTabItem(DummyData.GetPortfolioList()[2])}
        };

    }
}