using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManager.Repository.Entities
{
   [Table ("PriceHistory")]
    public class PriceHistory
    {
        [Key]
        public int PriceHistoryId { get; set; }
        public int InvestmentId { get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice { get; set; }
        public DateTime ValuationDate { get; set; }

        public DateTime RecordedDate { get; set; }
    }
}
