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

            this.TabsView.DataContext = new PortfolioListTabViewModel();
            this.TopLevelMenu.DataContext = new TopLevelMenuViewModel();
        }
    }
}
