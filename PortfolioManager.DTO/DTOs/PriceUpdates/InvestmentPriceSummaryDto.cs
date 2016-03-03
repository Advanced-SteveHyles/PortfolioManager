using System;

namespace Portfolio.Common.DTO.DTOs.PriceUpdates
{
    public class InvestmentPriceSummaryDto
    {
        public int InvestmentId { get; set; }

        public string InvestmentName { get; set; }
        public decimal LatestSellPrice { get; set; }
        public DateTime LatestSellPriceDate { get; set; }

        public decimal LatestBuyPrice { get; set; }
        public DateTime LatestBuyPriceDate { get; set; }        
    }
}
