using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using PortfolioManager.Model;

namespace PortfolioManager.UIBuilders
{
    public class PortfolioTabPanelViewModel
    {
        private readonly int _portfolioId;

        public List<TabItem> AccountTabs
        {
            get
            {
                return DummyData.FakeAccountData()
                    .Where(a => a.PortfolioId == _portfolioId)
                    .Select(accountdto => BuildPortfolioTabContent.CreateAccountTabItem(accountdto))
                    .ToList();
            }
        }


        public PortfolioTabPanelViewModel(int portfolioId)
        {
            _portfolioId = portfolioId;
        }
    }
}