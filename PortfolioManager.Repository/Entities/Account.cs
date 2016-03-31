﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.BackEnd.Repository.Entities
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
    }


}