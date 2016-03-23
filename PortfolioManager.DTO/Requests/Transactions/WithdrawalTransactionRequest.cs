using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class WithdrawalTransactionRequest
    {
        public int AccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Value { get; set; }

        public string Source { get; set; }
        
    }

    public class FeeTransactionRequest
    {
        public int AccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Value { get; set; }
        

    }
}