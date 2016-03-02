using System;
using System.ComponentModel.DataAnnotations.Schema;
using PortfolioManager.DTO.DTOs;

namespace PortfolioManager.Repository.Entities
{
    [Table ("AccountInvestmentMap")]
    public class AccountInvestmentMap
    {
        public int AccountInvestmentMapId { get; set; }

        public int AccountId { get; set; }
        public int InvestmentId { get; set; }
        public decimal Quantity { get; set; }        
        public decimal? Valuation { get; set; }
        
    }
}