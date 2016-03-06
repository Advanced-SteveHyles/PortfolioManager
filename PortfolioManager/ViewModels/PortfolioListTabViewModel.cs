using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager
{
    public class PortfolioListTabViewModel
    {        
        public List<TabItem> PortfolioTabs
        {
            get
            {
                return DummyData.GetPortfolioList()
                    .Select(portfolio => BuildPortfolioTabContent
                    .CreatePortfolioTabItem(portfolio)).ToList();
            }
        }
    }
}