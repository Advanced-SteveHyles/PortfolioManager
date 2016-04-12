using System.Windows.Controls;
using PortfolioManager.Interfaces;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages
{
    public class AccountInvestmentsTreeItem : ITreeBlock
    {
        private int _accountId;

        public AccountInvestmentsTreeItem(string title, int accountId)
        {
            Title = title;
            _accountId = accountId;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
        public UserControl GetView()
        {
            return new AccountInvestmentDetails() {DataContext = new AccountInvestmentDetailsViewModel(_accountId)};
        }
    }
}