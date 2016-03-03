using PagedList;
using Portfolio.Common.DTO.DTOs;
using PortfolioManagerWeb.Helpers;

namespace PortfolioManagerWeb.Models
{
    public class AccountsViewModel
    {
        public StaticPagedList<AccountDto> Accounts { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}