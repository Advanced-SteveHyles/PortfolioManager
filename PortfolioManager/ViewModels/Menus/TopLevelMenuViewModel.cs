using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.ViewModels.Menus
{
    public class TopLevelMenuViewModel:INotifyPropertyChanged
    {

        public object Bob{ get; set; }

        public ICommand AddPortfolioCommand => new RelayCommand(AddPortfolio);

        public void AddPortfolio()
        {
            var view = new PortfolioDataEntry() { DataContext = new PortfolioDataEntryViewModel(DialogClose) };
            Bob = view;
            OnPropertyChanged("Bob");
        }

        void DialogClose ()
        {
            Bob = null;
            OnPropertyChanged("Bob");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
