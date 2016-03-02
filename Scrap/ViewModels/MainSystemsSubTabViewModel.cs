using Interfaces;
using WPFBase.ViewModels;

namespace PortfolioManager
{
    public class MainSystemsSubTabViewModel : TabViewViewModel
    {
        public MainSystemsSubTabViewModel(IIOCContainer rep)
        {          
        }

        public override string GetTabName
        {
            get { return "MainSystemsSubTabViewModel"; }
        }
    }
}
