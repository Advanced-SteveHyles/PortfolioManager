using System.Windows.Controls;
using PortfolioManager.Interfaces;
using PortfolioManager.ViewModels;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages
{
    public class AccountTransactionTreeItem : ITreeBlock
    {
        private readonly int _accountId;

        public AccountTransactionTreeItem(string title, int accountId)
        {
            _accountId = accountId;
            Title = title;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
        public UserControl GetView()
        { 
            return new AccountTransactionSummary() {DataContext = new AccountTransactionSummaryViewModel(_accountId) };        
        }        
    }
}