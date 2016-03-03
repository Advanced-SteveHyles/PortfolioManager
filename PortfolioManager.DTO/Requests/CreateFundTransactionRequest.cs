using System;

namespace PortfolioManager.DTO.Requests
{
    public class CreateFundTransactionRequest
    {
        public int InvestmentMapId { get; set; }

        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime SettlementDate { get; set; }

        public string Source { get; set; }

        public decimal Quantity { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? BuyPrice { get; set; }

        public decimal Charges { get; set; }
        public decimal TransactionValue { get; set; }
    }
}