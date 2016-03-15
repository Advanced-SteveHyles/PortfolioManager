using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;
using PortfolioManager.Model.Decorators;

namespace PortfolioManager.Model
{
    public static class InvestmentModel
    {
        public static List<InvestmentDto> GetInvestments()
        {
            var service = new VirtualInvestmentsController();
            var investments = service.GetAllInvestments();
            return investments?.ToList() ?? new List<InvestmentDto>();
        }

        public static void InsertInvestment(InvestmentRequest investmentRequest)
        {
            var service = new VirtualInvestmentsController();
            service.InsertInvestment(investmentRequest);
        }

        public static List<PriceHistoryDecorator>  GetInvestmentsForPriceUpdate()
        {
            return GetInvestments().Select(investment => new PriceHistoryDecorator(investment)).ToList();
        }        
    }
}