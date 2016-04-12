using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Model;
using PortfolioManager.TreePages;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views.TabPanels;

namespace PortfolioManager.ViewModels
{    
    public class PortfolioBlock : ITreeBlock
    {
        private readonly PortfolioDto _portfolio;

        public ObservableCollection<AccountBlock> AccountBlocks { get; set; } = new ObservableCollection<AccountBlock>();

        public PortfolioBlock(PortfolioDto portfolio)
        {
            _portfolio = portfolio;

            foreach (var a in AccountModel.GetAccountForPortfolio(_portfolio.PortfolioId))
            {
                AccountBlocks.Add(new AccountBlock(a));
            }

                  //.Where(a => a.PortfolioId == _portfolioId)
                  //.Select(accountdto => BuildPortfolioTabContent.CreateAccountTabItem(accountdto))
                  //.ToList();
        }

        public string Title => _portfolio.Name;
        public int Rating { get; set; }


        public UserControl View()
        {
            return new PortfolioDetailsView();
        }
    }
}