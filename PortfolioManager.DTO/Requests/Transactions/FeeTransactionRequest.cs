using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class FeeTransactionRequest : ITransactionRequest
    {
        public int AccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Value { get; set; }        
    }
}