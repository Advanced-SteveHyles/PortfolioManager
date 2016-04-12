using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Interfaces;
using PortfolioManager.TreePages;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager.ViewModels
{
    public class AccountTreeItem: ITreeBlock
    {
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
            return new AccountDetailsView() {DataContext = new AccountDetailsViewModel(AccountDto)};
        }
    }
}