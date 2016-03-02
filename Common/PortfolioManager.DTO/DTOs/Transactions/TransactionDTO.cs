using System;

namespace PortfolioManager.DTO.DTOs.Transactions
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }

        public string TransactionType { get; set; }

        public DateTime? TransactionDate { get; set; }
        public string Source { get; set; }
        public decimal Value { get; set; }
        public bool IsTaxRefund { get; set; }
    }
}