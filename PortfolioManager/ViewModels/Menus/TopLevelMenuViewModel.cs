using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PortfolioManager.ViewModels.Menus
{ 
    public class TopLevelMenuViewModel: ViewModel
    {
        const string DynamicContentControl = "DynamicContent";

        public object DynamicContent{ get; set; }

        public ICommand AddPortfolioCommand => new RelayCommand(AddPortfolio);
        public ICommand AddInvestmentCommand => new RelayCommand(AddInvestment);

        public void AddPortfolio()
        {
            DynamicContent = BuildDynamicMenu.PortFolioDataEntryView(DialogClose);
            OnPropertyChanged(DynamicContentControl);
        }

        public void AddInvestment()
        {
            DynamicContent = BuildDynamicMenu.InvestmentDataEntryView(DialogClose);
            OnPropertyChanged(DynamicContentControl);
        }

        private void DialogClose ()
        {
            DynamicContent = null;            
            OnPropertyChanged(DynamicContentControl);
        }        
    }
}
