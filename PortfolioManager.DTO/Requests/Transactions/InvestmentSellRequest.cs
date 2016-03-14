using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class InvestmentSellRequest
    {
        public int InvestmentMapId { get; set; }
        public decimal Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Value { get; set; }
        public decimal Charges { get; set; }

        public DateTime SellDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public bool UpdatePriceHistory { get; set; } = true;
    }
}