using System.Windows.Controls;
using PortfolioManager.Interfaces;
using PortfolioManager.ViewModels;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages
{
    public class AccountTransactionTreeItem : ITreeBlock
    {
        private readonly int _accountId;
        private AccountTransactionSummaryViewModel _accountTransactionSummaryViewModel;

        public AccountTransactionTreeItem(string title, int accountId)
        {
            _accountId = accountId;
            Title = title;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
        public UserControl GetView()
        {
            if (_accountTransactionSummaryViewModel ==null)
                _accountTransactionSummaryViewModel = new AccountTransactionSummaryViewModel(_accountId);

            return new AccountTransactionSummary() {DataContext = _accountTransactionSummaryViewModel };
        }
    }
}