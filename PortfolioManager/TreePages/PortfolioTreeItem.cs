using System.Collections.ObjectModel;
using System.Windows.Controls;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;
using PortfolioManager.TreePages.FirstLevelItems;
using PortfolioManager.ViewModels;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages
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
        
        public UserControl GetView()
        {
            if (_portfolioDetailsViewModel == null)
                _portfolioDetailsViewModel = new PortfolioDetailsViewModel(_portfolio.PortfolioId);

            return new PortfolioDetailsView() {DataContext = _portfolioDetailsViewModel};
        }
    }
}