using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Interfaces;

namespace WPFBase.ViewModels
{
   public abstract class ViewModel : INotifyPropertyChanged
    {
     protected IIOCContainer IOCC;
       public ViewModel() { }
       public ViewModel(IIOCContainer iocc)
       {
           this.IOCC = iocc;
       }

       public event PropertyChangedEventHandler PropertyChanged;

       // This method is called by the Set accessor of each property. 
       // The CallerMemberName attribute that is applied to the optional propertyName 
       // parameter causes the property name of the caller to be substituted as an argument. 
       protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
    }
}
