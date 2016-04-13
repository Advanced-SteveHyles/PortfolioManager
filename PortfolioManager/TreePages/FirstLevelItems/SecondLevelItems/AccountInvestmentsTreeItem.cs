using System.Windows.Controls;
using PortfolioManager.Interfaces;
using PortfolioManager.ViewModels;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages.FirstLevelItems.SecondLevelItems
{
    public class AccountInvestmentsTreeItem : ITreeBlock
    {
        private int _accountId;
        private AccountInvestmentDetailsViewModel _accountInvestmentDetailsViewModel;

        public AccountInvestmentsTreeItem(string title, int accountId)
        {
            Title = title;
            _accountId = accountId;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
        public UserControl GetView()
        {
            if (_accountInvestmentDetailsViewModel ==null)
                _accountInvestmentDetailsViewModel = new AccountInvestmentDetailsViewModel(_accountId);

            return new AccountInvestmentDetails() {DataContext = _accountInvestmentDetailsViewModel};
        }
    }
}