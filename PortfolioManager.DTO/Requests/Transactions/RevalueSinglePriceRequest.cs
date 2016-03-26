using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class RevalueSinglePriceRequest : ITransactionRequest
    {
        public int InvestmentId { get; set; }

        public DateTime ValuationDate { get; set; }
        
    }
}