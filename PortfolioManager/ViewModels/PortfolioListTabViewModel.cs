using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;

namespace PortfolioManager.ViewModels
{
    public class PortfolioListTabViewModel
    {                
        public List<TabItem> PortfolioTabs
        {
            get
            {
                return PortfolioModel.GetPortfolioList()
                    .Select(portfolio => BuildPortfolioTabContent
                    .CreatePortfolioTabItem(portfolio)).ToList();
            }
        }
    }
}