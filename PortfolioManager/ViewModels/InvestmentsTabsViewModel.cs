using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Portfolio.API.Virtual.VirtualControllers;
using PortfolioManager.ViewModels;
using PortfolioManager.Views.DataEntry;
using PortfolioManager.Views.TabPanels;

namespace PortfolioManager
{
    public class InvestmentsTabsViewModel
    {
        public object InvestmentList
        {
            get { return new InvestmentTabPanel() { DataContext = new InvestmentTabPanelViewModel() }; }
        }


    }
}