using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class PriceHistoryRequest : ITransactionRequest
    {
        public int InvestmentId { get; set; }
        public DateTime ValuationDate { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? BuyPrice { get; set; }
    }
}