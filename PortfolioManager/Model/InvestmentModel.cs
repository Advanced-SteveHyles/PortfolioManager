﻿using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualActionResults;
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
            var investments = service.GetAllInvestments() as OkMultipleActionResult<InvestmentDto>;
            return investments?.EnumerateObjectInstances.ToList() ?? new List<InvestmentDto>();
        }

        public static void InsertInvestment(InvestmentRequest investmentRequest)
        {
            var service = new VirtualInvestmentsController();
            service.InsertInvestment(investmentRequest);
        }

        public static List<PriceHistoryDecorator>  GetInvestmentsForPriceUpdate()
        {
            var decoratedInvestments = new List<PriceHistoryDecorator>();

            foreach (var Investment in GetInvestments())
            {
                decoratedInvestments.Add(new PriceHistoryDecorator(Investment));
            }

            return decoratedInvestments;
        }
    }
}