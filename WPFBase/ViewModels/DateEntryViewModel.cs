using System.ComponentModel;
using Interfaces;

namespace WPFBase.ViewModels
{
    public class DateEntryViewModel : ViewModel
    {
        private DateEntryViewModel() { }
        public DateEntryViewModel(IIOCContainer rep) : base (rep)
        {}
    }
}
