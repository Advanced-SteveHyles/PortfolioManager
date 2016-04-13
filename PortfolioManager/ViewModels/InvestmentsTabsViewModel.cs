using PortfolioManager.Views.TabPanels;

namespace PortfolioManager.ViewModels
{
    public class InvestmentsTabsViewModel
    {
        public object InvestmentList
        {
            get { return new InvestmentTabPanel() { DataContext = new InvestmentTabPanelViewModel() }; }
        }


    }
}