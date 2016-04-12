using System.Collections.ObjectModel;
using System.Linq;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;

namespace PortfolioManager.ViewModels
{
    public class PortfolioDriver
    {

        public ObservableCollection<PortfolioTreeItem> PortfolioBlocks { get; set; } = new ObservableCollection<PortfolioTreeItem>();
        

        public PortfolioDriver()
        {
            foreach (var x in PortfolioModel.GetPortfolioList())
            {
                PortfolioBlocks.Add(new PortfolioTreeItem(x));
            }
        }
    }
}