using System;

namespace Portfolio.Common.DTO.DTOs.Transactions
{
    public class CashTransactionDto
    {
        public int CashTransactionId { get; set; }

        public int AccountId { get; set; }

        public string TransactionType { get; set; }

        public DateTime? TransactionDate { get; set; }
        public string Source { get; set; }
        public decimal TransactionValue { get; set; }
        public bool IsTaxRefund { get; set; }
    }
}