using PagedList;
using Portfolio.Common.DTO.DTOs;
using PortfolioManagerWeb.Helpers;

namespace PortfolioManagerWeb.Models
{
    public class PortfoliosViewModel
    {
        public StaticPagedList<PortfolioDto> Portfolios { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}