using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class CashTransferRequest
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}