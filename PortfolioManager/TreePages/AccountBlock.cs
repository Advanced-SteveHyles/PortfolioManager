using System.Collections.ObjectModel;
using System.Windows.Controls;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.TreePages;
using PortfolioManager.UIBuilders;
using PortfolioManager.Views;

namespace PortfolioManager.ViewModels
{
    public class AccountBlock: ITreeBlock
    {
        public AccountDto Dto { get; set; }
        
        public ObservableCollection<AccountSubBlock> AccountSubBlocks { get; set; } = new ObservableCollection<AccountSubBlock>();
        
        public AccountBlock(AccountDto dto)
        {
            Dto = dto;

            AccountSubBlocks.Add(new AccountSubBlock("SB1", -1));
            AccountSubBlocks.Add(new AccountSubBlock("SB2", 2));
            AccountSubBlocks.Add(new AccountSubBlock("SB3", 3));
        }

        public string Title => Dto.Name;
        public int Rating { get; set; }
        public UserControl View()
        {
            return new AccountDetailsView();
        }
    }
}