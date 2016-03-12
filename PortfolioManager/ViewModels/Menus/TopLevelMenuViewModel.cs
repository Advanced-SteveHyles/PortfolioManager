using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels.Menus
{ 
    public class TopLevelMenuViewModel: ViewModel
    {
        const string DynamicContentControl = "DynamicContent";

        public object DynamicContent{ get; set; }

        public ICommand AddPortfolioCommand => new RelayCommand(AddPortfolio);
        public ICommand AddInvestmentCommand => new RelayCommand(AddInvestment);

        public ICommand UpdateAllPricesCommand => new RelayCommand(UpdateAllPrices);

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

        public void UpdateAllPrices()
        {
            PriceHistoryModel.UpdatePriceHistories();
        }

        private void DialogClose ()
        {
            DynamicContent = null;            
            OnPropertyChanged(DynamicContentControl);
        }     
        
        InitialLoad
        Decimal (18,4)   
        ViewAccount
        DataEntryForPriceUpdates
    }
}
