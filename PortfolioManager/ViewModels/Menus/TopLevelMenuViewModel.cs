using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using PortfolioManager.Model;
using PortfolioManager.Other;

namespace PortfolioManager.ViewModels.Menus
{ 
    public class TopLevelMenuViewModel: ViewModel
    {
        const string DynamicContentControl = "DynamicContent";

        public object DynamicContent{ get; set; }

        public ICommand AddPortfolioCommand => new RelayCommand(AddPortfolio);
        public ICommand AddInvestmentCommand => new RelayCommand(AddInvestment);

        public ICommand RevalueFundsCommand => new RelayCommand(RevalueFunds);

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

        private void UpdateAllPrices()
        {
            DynamicContent = BuildDynamicMenu.PriceHistoryDataEntryView(DialogClose);
            OnPropertyChanged(DynamicContentControl);
        }


        public void RevalueFunds()
        {
            PriceHistoryModel.UpdatePriceHistories();
        }

        private void DialogClose ()
        {
            DynamicContent = null;            
            OnPropertyChanged(DynamicContentControl);
        }     
                
    }
}
