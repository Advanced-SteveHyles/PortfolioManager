using System;

namespace PortfolioManager.DTO.Requests.Transactions
{
    public class WithdrawalTransactionRequest
    {
        public int AccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Value { get; set; }

        public string Source { get; set; }
        
    }
}