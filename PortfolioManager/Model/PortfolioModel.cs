using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var portfolios = service.Get() as OkMultipleActionResult<PortfolioDto>;
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

        internal static List<AccountInvestmentMapDto> FakePopulatedInvestmentMap()
        {
            return new List<AccountInvestmentMapDto>
            {
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 1,
                    InvestmentId = 1,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 2,
                    InvestmentId = 1,
                    AccountId = 2,
                    Quantity = 5,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 3,
                    InvestmentId = 2,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 4,
                    InvestmentId = 1,
                    AccountId = 3,
                    Quantity = (decimal)25.4,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 88,
                    InvestmentId = 1,
                    AccountId = 4,
                    Quantity = (decimal)1.78923,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 89,
                    InvestmentId = 3,
                    AccountId = 6,
                    Quantity = 21,
                },
            };
        }
    }

}
