using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;

namespace PortfolioManager.ViewModels
{
    public class PortfolioListTabViewModel
    {                
        public List<TabItem> PortfolioTabs
        {
            get
            {
                return PortfolioModel.GetPortfolioList()
                    .Select(portfolio => BuildPortfolioTabContent
                    .CreatePortfolioTabItem(portfolio)).ToList();
            }
        }

        public List<TreeViewItem> PortfolioTree
        {
            get
            {
                return PortfolioModel.GetPortfolioList()
                    .Select(portfolio => BuildPortfolioTabContent
                    .CreatePortfolioTreeViewItem(portfolio)).ToList();
            }
        }

        public PortfolioDriver PortfolioTreeDataContext
        {
            get { return new PortfolioDriver(); }
        }
    }

    public class PortfolioDriver
    {

        public object PortfolioBlocks
        { get {return new Top} } 
    }

    public class FundTopic //: ITopic
    {

        public FundTopic(string funds, int i)
        {
            Title = funds;
            Rating = i;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
    }

}