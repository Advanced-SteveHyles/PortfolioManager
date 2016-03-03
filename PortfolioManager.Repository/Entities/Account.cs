using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManager.Repository.Entities
{
    [Table ("Account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        public string Name { get; set; }

        public decimal Cash { get; set; }
        public decimal Valuation { get; set; }

        public int PortfolioId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<AccountInvestmentMap> Investments { get; set; }
    }
}