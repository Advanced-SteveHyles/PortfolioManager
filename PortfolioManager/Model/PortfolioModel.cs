using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace PortfolioManager.Model
{
    public static class PortfolioModel
    {
        public static List<PortfolioDto> GetPortfolioList()
        {
            var service = new VirtualPortfoliosController(ApiConstants.VirtualApiPortfoliomanagercontext);
            var portfolios = service.GetPortfolios() as OkMultipleActionResult<PortfolioDto>;
            return portfolios?.EnumerateObjectInstances.ToList() ?? new List<PortfolioDto>();
        }

        public static void InsertPortfolio(PortfolioRequest portfolioRequest)
        {
            var service = new VirtualPortfoliosController(ApiConstants.VirtualApiPortfoliomanagercontext);
            var portfolio = service.Post(portfolioRequest) as CreatedActionResult<PortfolioDto>;

            if (portfolio == null)
            {
                throw new InvalidOperationException();
            }
        }
    }

}
