using System;

namespace PortfolioManager.DTO.Requests
{
    public class CreateCashTransactionRequest
    {
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionValue { get; set; }
        public string Source { get; set; }
        public bool IsTaxRefund { get; set; }
        public string TransactionType { get; set; }
    }
}