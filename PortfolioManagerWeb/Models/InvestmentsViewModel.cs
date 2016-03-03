using System.Collections;
using PagedList;
using PortfolioManager.DTO;
using PortfolioManagerWeb.Helpers;

namespace PortfolioManagerWeb.Models
{
    public class InvestmentsViewModel
    {
        public PagingInfo PagingInfo { get; set; }    
        public StaticPagedList<InvestmentDto> Investments { get; set; }
    }
}