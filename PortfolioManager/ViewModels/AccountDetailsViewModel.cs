using System.Windows.Controls;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Interfaces;
using PortfolioManager.Other;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.ViewModels
{
    public class AccountDetailsViewModel : ViewModel
    {
        private readonly AccountDto _account;
        private const string CashTransactionName = "CashTransaction";
        private UserControl _cashTransaction;
        public UserControl CashTransaction => this._cashTransaction;

        public AccountDetailsViewModel(AccountDto account)
        {
            this._account = account;
        }

        public int AccountId => _account.AccountId;
        public string Name => _account.Name;
        public int PortfolioId => _account.PortfolioId;
        public decimal Cash => _account.Cash;
        public decimal Valuation => _account.Valuation;
        public string Type => _account.Type;
        public decimal AccountBalance => _account.AccountBalance;


        public ICommand DepositCommand => new RelayCommand(Deposit);
        public ICommand WithdrawalCommand => new RelayCommand(Withdrawal);
        public ICommand TransferCommandOut => new RelayCommand(TransferOut);

        public ICommand TransferCommandIn => new RelayCommand(TransferIn);
        

        private void Deposit()
        {
            _cashTransaction = new CashDepositView()
            {
                DataContext = new CashDepositViewModel(_account.AccountId, CompleteTransaction)
            };
            OnPropertyChanged(CashTransactionName);
        }

        private void Withdrawal()
        {
            _cashTransaction = new CashWithdrawalView()
            {
                DataContext = new CashWithdrawalViewModel(_account.AccountId, CompleteTransaction)
            };
            OnPropertyChanged(CashTransactionName);
        }
     
        private void TransferOut()
        {
            _cashTransaction = new CashTransferView()
            {
                DataContext = CashTransferViewModel.CreateCashTransferOutViewModel( _account.AccountId, CompleteTransaction)
            };
            OnPropertyChanged(CashTransactionName);            
        }

        private void TransferIn()
        {
            _cashTransaction = new CashTransferView()
            {
                DataContext = CashTransferViewModel.CreateCashTransferInViewModel(_account.AccountId, CompleteTransaction)
            };
            OnPropertyChanged(CashTransactionName);            
        }


        private void CompleteTransaction()
        {
            _cashTransaction = null;
            OnPropertyChanged(CashTransactionName);
        }
    }
}