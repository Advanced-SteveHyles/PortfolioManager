using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Model.Decorators;

namespace PortfolioManager.Model
{
    public static class PriceHistoryModel
    {
        public static void UpdatePriceHistories()
        {            
            var service = new VirtualPriceHistoryController();
            service.UpdateAllPrices();
        }

        public static void MassSavePriceHistories(List<PriceHistoryDecorator> investments)
        {
            var service = new VirtualPriceHistoryController();
            var requests =  investments.Select(ph => new PriceHistoryRequest()
            {
                InvestmentId = ph.InvestmentId,
                BuyPrice = ph.BuyPrice,
                SellPrice = ph.SellPrice,
                ValuationDate = DateTime.Now
            });

            service.SavePriceHistories(requests);
        }
    }
}