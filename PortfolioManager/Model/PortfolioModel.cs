using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace PortfolioManager.Model
{
    public static class PortfolioModel
    {
        public static List<PortfolioDto> GetPortfolioList()
        {
            var service = new VirtualPortfoliosController();
            var portfolios = service.GetPortfolios();
            return portfolios?.ToList() ?? new List<PortfolioDto>();
        }

        public static PortfolioDto InsertPortfolio(PortfolioRequest portfolioRequest)
        {
            var service = new VirtualPortfoliosController();
            var portfolio = service.InsertPortfolio(portfolioRequest);

            return portfolio;
        }
    }

}
