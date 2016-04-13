using System.Windows.Controls;
using System.Windows.Input;
using PortfolioManager.Interfaces;
using PortfolioManager.Other;
using PortfolioManager.TreePages;
using PortfolioManager.Views;
using PortfolioManager.Views.TabControls;

namespace PortfolioManager.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private UserControl _mainContentArea;
        private PortfolioTree _portfolioTabsList;
        private InvestmentsTabsList _investmentTabsList;

        public UserControl MainContentArea
        {
            get { return _mainContentArea; }
            set
            {
                _mainContentArea = value;
                OnPropertyChanged("MainContentArea");
            }
        }

        public ICommand RefreshCommand => new RelayCommand(Refresh);

        private void Refresh()
        {
            _portfolioTabsList = null;
            _investmentTabsList = null;
        }


        public ICommand ShowPortfolioScreenCommand => new RelayCommand(ShowPortfolioScreen);

        private void ShowPortfolioScreen()
        {
            if (_portfolioTabsList == null)
            { 
                _portfolioTabsList = new PortfolioTree() {DataContext = new PortfolioTreeViewModel()};
            }
            MainContentArea = _portfolioTabsList;
        }

        public ICommand ShowInvestmentScreenCommand => new RelayCommand(ShowInvestmentScreen);

        private void ShowInvestmentScreen()
        {
            if (_investmentTabsList == null)
            {
                _investmentTabsList = new InvestmentsTabsList(){ DataContext = new InvestmentsTabsViewModel()};
            }
            MainContentArea = _investmentTabsList;
        }
    }
}