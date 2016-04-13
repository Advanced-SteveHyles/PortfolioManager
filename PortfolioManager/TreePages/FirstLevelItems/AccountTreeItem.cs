using System.Collections.ObjectModel;
using System.Windows.Controls;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Interfaces;
using PortfolioManager.TreePages.FirstLevelItems.SecondLevelItems;
using PortfolioManager.ViewModels;
using PortfolioManager.Views;

namespace PortfolioManager.TreePages.FirstLevelItems
{
    public class AccountTreeItem: ITreeBlock
    {
        private AccountDetailsViewModel _accountDetailsViewModel;
        private AccountDto AccountDto { get; }
        
        public ObservableCollection<ITreeBlock> AccountSubBlocks { get; set; } = new ObservableCollection<ITreeBlock>();
        
        public AccountTreeItem(AccountDto accountDto)
        {
            this.AccountDto = accountDto;

            AccountSubBlocks.Add(new AccountTransactionTreeItem("Transactions", accountDto.AccountId));
            AccountSubBlocks.Add(new AccountInvestmentsTreeItem("Investments", accountDto.AccountId));
            AccountSubBlocks.Add(new AccountTransactionTreeItem("SB3",0));
        }

        public string Title => AccountDto.Name;
        public int Rating { get; set; }
        public UserControl GetView()
        {
            if (_accountDetailsViewModel == null)
                _accountDetailsViewModel = new AccountDetailsViewModel(AccountDto);

            return new AccountDetailsView() {DataContext = _accountDetailsViewModel};
        }
    }
}