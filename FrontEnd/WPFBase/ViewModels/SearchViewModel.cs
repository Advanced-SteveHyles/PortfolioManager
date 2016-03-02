using Interfaces;

namespace WPFBase.ViewModels
{
    public class SearchViewModel : ViewModel
    {
         private SearchViewModel() { }
         public SearchViewModel(IIOCContainer rep)
             : base(rep)
        {}
    }
}
