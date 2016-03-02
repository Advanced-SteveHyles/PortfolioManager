using System;

namespace PortfolioManager.DTO.Transactions
{
    public class InvestmentBuyRequest
    {
        public int InvestmentMapId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public decimal Charges { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public bool UpdatePriceHistory { get; set; } = true;
    }
}