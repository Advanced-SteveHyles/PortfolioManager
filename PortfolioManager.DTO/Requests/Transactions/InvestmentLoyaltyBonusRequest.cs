using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class InvestmentLoyaltyBonusRequest : ITransactionRequest
    {
        public int InvestmentMapId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }        
    }
}