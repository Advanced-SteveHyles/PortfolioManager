using System.Collections.ObjectModel;
using PortfolioManager.Model;
using PortfolioManager.ViewModels;

namespace PortfolioManager.TreePages
{
    public class PortfolioTreeViewModel
    {

        public ObservableCollection<PortfolioTreeItem> PortfolioBlocks { get; set; } = new ObservableCollection<PortfolioTreeItem>();
        

        public PortfolioTreeViewModel()
        {
            foreach (var x in PortfolioModel.GetPortfolioList())
            {
                PortfolioBlocks.Add(new PortfolioTreeItem(x));
            }
        }
    }
}