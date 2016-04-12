using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PortfolioManager.TreePages;
using PortfolioManager.ViewModels;

namespace PortfolioManager.Views
{
    /// <summary>
    /// Interaction logic for PortfolioTree.xaml
    /// </summary>
    public partial class PortfolioTree : UserControl
    {
        public PortfolioTree()
        {
            InitializeComponent();
        }

        private void MyTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var y = e.NewValue as ITreeBlock;
            if (y != null)
            {
                this.Placeholder.Content= y.View();
            }            
        }
    }
}
