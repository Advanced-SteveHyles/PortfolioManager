using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views.TabPanels;

namespace PortfolioManager.ViewModels
{    
    public class PortfolioTreeItem : ITreeBlock
    {
        private readonly PortfolioDto _portfolio;
        private PortfolioDetailsViewModel _portfolioDetailsViewModel;

        public ObservableCollection<AccountTreeItem> AccountBlocks { get; set; } = new ObservableCollection<AccountTreeItem>();

        public PortfolioTreeItem(PortfolioDto portfolio)
        {
            _portfolio = portfolio;

            foreach (var a in AccountModel.GetAccountForPortfolio(_portfolio.PortfolioId))
            {
                AccountBlocks.Add(new AccountTreeItem(a));
            }
        }

        public string Title => _portfolio.Name;
        public int Rating { get; set; }


        public UserControl GetView()
        {
            if (_portfolioDetailsViewModel == null)
                _portfolioDetailsViewModel = new PortfolioDetailsViewModel();

            return new PortfolioDetailsView() {DataContext = _portfolioDetailsViewModel};
        }
    }
}