using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PortfolioManager.Other;
using PortfolioManager.ViewModels;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views;
using PortfolioManager.Views.DataEntry;
using PortfolioManager.Views.TabControls;

namespace PortfolioManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();           
            this.TopLevelMenu.DataContext = new TopLevelMenuViewModel();
        }
    }

    public class MainWindowViewModel : ViewModel
    {
        private UserControl _mainContentArea;
        private PortfolioTabsList _portfolioTabsList;
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

        public ICommand ShowPortfolioScreenCommand => new RelayCommand(ShowPortfolioScreen);

        private void ShowPortfolioScreen()
        {
            if (_portfolioTabsList == null)
            { 
                _portfolioTabsList = new PortfolioTabsList() {DataContext = new PortfolioListTabViewModel()};
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
