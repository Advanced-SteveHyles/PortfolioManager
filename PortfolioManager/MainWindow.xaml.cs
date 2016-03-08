using System.Windows;
using PortfolioManager.ViewModels.Menus;

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

            this.PortfolioTabsView.DataContext = new PortfolioListTabViewModel();
            this.InvestmentsTabsView.DataContext = new InvestmentsTabsViewModel();
            this.TopLevelMenu.DataContext = new TopLevelMenuViewModel();
        }
    }
}
