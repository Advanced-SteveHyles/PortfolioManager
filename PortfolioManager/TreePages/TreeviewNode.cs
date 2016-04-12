using System.Collections.ObjectModel;

namespace PortfolioManager.ViewModels
{
    public class TreeviewNode
    {
        public ObservableCollection<TreeviewNode> AccountBlocks { get; set; } = new ObservableCollection<TreeviewNode>();


        public TreeviewNode(string funds, int i)
        {

            Title = funds;
            Rating = i;
        }
        
        public string Title { get; set; }
        public int Rating { get; set; }
    }
}