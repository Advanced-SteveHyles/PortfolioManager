using System;
using PortfolioManager.DTO.DTOs.PriceUpdates;

namespace PortfolioManagerWeb.Models
{
    public class InvestmentPriceSummaryDecorator
    {
        public InvestmentPriceSummaryDto InvestmentPriceSummary { get; set; }
        public string NewBuyPrice { get; set; }
        public string NewSellPrice { get; set; }

        public DateTime ValuationDate { get; set; }
    }

}
