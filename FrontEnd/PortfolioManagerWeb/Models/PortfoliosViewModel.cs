using PagedList;
using PortfolioManager.DTO;
using PortfolioManagerWeb.Helpers;

namespace PortfolioManagerWeb.Models
{
    public class PortfoliosViewModel
    {
        public StaticPagedList<PortfolioDto> Portfolios { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}