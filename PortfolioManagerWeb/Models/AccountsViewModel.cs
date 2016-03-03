using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PortfolioManager.DTO;
using PortfolioManagerWeb.Helpers;

namespace PortfolioManagerWeb.Models
{
    public class AccountsViewModel
    {
        public StaticPagedList<AccountDto> Accounts { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}