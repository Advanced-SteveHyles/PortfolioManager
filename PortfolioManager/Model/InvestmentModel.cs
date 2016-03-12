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
    public static class InvestmentModel
    {
        public static List<InvestmentDto> GetInvestments()
        {
            var service = new VirtualInvestmentsController(ApiConstants.VirtualApiPortfoliomanagercontext);
            var investments = service.GetAllInvestments() as OkMultipleActionResult<InvestmentDto>;
            return investments?.EnumerateObjectInstances.ToList() ?? new List<InvestmentDto>();
        }

        public static void InsertInvestment(InvestmentRequest investmentRequest)
        {
            var service = new VirtualInvestmentsController(ApiConstants.VirtualApiPortfoliomanagercontext);
            service.InsertInvestment(investmentRequest);
        }
    }
}