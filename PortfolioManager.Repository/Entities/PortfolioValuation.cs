using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.BackEnd.Repository.Entities
{
    [Table("PortfolioValuation")]
    public class PortfolioValuation
    {
        [Key]
        public int PortfolioValuationId { get; set; }

        public int PortfolioId { get; set; }
        public decimal PropertyValue { get; set; }
        public decimal PropertyRatio { get; set; }
        public decimal CashValue { get; set; }

        public decimal CashRatio { get; set; }

        public decimal BondValue { get; set; }
        public decimal BondRatio { get; set; }
        public decimal EquityValue { get; set; }
        public decimal EquityRatio { get; set; }        
    }
}