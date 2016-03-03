using Interfaces;
using System;
using System.Windows.Input;
using WPFBase.ViewModels;

namespace WPFBase.Components
{    
    public sealed class CrudViewModel : ViewModel, ICrudViewModel
    {
        private CrudViewModel() : base(null) { }
        public CrudViewModel(IIOCContainer rep) : base(rep) 
        {            
        }

        public void SetNewCmd(Action<object> execute, Predicate<object> canExecute)
        {
            _NewCmd = new RelayCommand(execute, canExecute, "new");
            NotifyPropertyChanged("NewCmd");
        }

        public void SetSaveCmd(Action<object> execute, Predicate<object> canExecute)
        {
            _SaveCmd = new RelayCommand(execute, canExecute, "Save");
            NotifyPropertyChanged("SaveCmd");
        }

        public void SetSelectCmd(Action<object> execute, Predicate<object> canExecute)
        {
            _SelectCmd = new RelayCommand(execute, canExecute, "Select");
            NotifyPropertyChanged("SelectCmd");
        }

        public int ShowSelectCmd()
        {
            if (_SelectCmd != null) { return 0; }
            return 2;
        }

        private ICommand _SelectCmd= new RelayCommand(null, null, string.Empty) ;
        public ICommand  SelectCmd{get {return _SelectCmd;}}

        private ICommand _NewCmd = new RelayCommand(null, null, string.Empty);
        public ICommand  NewCmd {get {return _NewCmd;}}


        private ICommand _SaveCmd = new RelayCommand(null, null, string.Empty);
        public ICommand SaveCmd { get { return _SaveCmd; } }

        private ICommand _CancelCmd = new RelayCommand(null, null, string.Empty);
        public ICommand CancelCmd { get { return _CancelCmd; } }

        private ICommand _DeleteCmd = new RelayCommand(null, null, string.Empty);
        public ICommand DeleteCmd { get { return _DeleteCmd; } }


        public void Refresh()
        {
           // SaveCmd.OnCanExecuteChanged();
        //    SaveCmd.CanExecuteChanged;            
            NotifyPropertyChanged("SaveCmd");
        }
    }
}
