using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManager.Repository.Entities
{
    [Table ("CashTransaction")]
    public class CashTransaction
    {
        [Key]
        public int CashTransactionId { get; set; }

        public int AccountId { get; set; }

        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }
        public string Source { get; set; }
        public decimal TransactionValue { get; set; }
        public bool IsTaxRefund { get; set; }
    }
}