using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class WithdrawalTransactionRequest : ITransactionRequest
    {
        public int AccountId { get; set; }

        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Value { get; set; }

        public string Source { get; set; }
        
    }
}