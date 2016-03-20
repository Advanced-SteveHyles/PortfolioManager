using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;
using PortfolioManager.Model;
using PortfolioManager.Other;
using PortfolioManager.ViewModels;
using PortfolioManager.ViewModels.Menus;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.UIBuilders
{
    public class AccountTabPanelViewModel : ViewModel
    {
        private const string CashTransactionName = "CashTransaction";
        private readonly AccountDto _account;
        private AccountInvestmentDetailsViewModel _accountInvestmentDetailsVm;

        public int AccountId => _account.AccountId;
        public string Name => _account.Name;
        public int PortfolioId => _account.PortfolioId;
        public decimal Cash => _account.Cash;
        public decimal Valuation => _account.Valuation;
        public string Type => _account.Type;
        public decimal AccountBalance => _account.AccountBalance;


        public AccountTabPanelViewModel(int accountId)
        {
            _account = AccountModel.GetAccount(accountId);
        }

        public ICommand DepositCommand => new RelayCommand(Deposit);

        public AccountInvestmentDetailsViewModel AccountInvestmentDetailsVm
        {
            get
            {
                if (_accountInvestmentDetailsVm == null)
                    _accountInvestmentDetailsVm = new AccountInvestmentDetailsViewModel(_account.AccountId);
                return _accountInvestmentDetailsVm;
            }
        }

        public ObservableCollection<CashTransactionDto> AccountTransactions
        {
            get
            {
                var accountTransactions = AccountModel.GetAccountTransactions(_account.AccountId);
                return new ObservableCollection<CashTransactionDto>(accountTransactions);
            }
        }

        private UserControl _cashTransaction;
        public UserControl CashTransaction => this._cashTransaction;

        private void Deposit()
        {
            _cashTransaction = new CashDepositView()
            {
                DataContext = new CashDepositViewModel(_account.AccountId, CompleteTransaction)
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