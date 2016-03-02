using Interfaces;

namespace WPFBase.ViewModels
{
    public abstract class TabViewViewModel : ViewModel
    {

        public TabViewViewModel() : base(null) { }
        public TabViewViewModel(IIOCContainer rep) : base(rep) { }

      public abstract string GetTabName { get; }
        
       
     public   string TabName
        { 
            get
            {
                return this.GetTabName;
            }          
        }       

    }
}
