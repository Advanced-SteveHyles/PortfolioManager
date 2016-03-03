using System.Collections.Generic;

namespace Portfolio.Common.DTO.DTOs
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string Name { get; set; }

        public int PortfolioId { get; set; }

        public decimal Cash { get; set; }
        public decimal Valuation { get; set; }

        public string Type { get; set; }

        public ICollection<AccountInvestmentMapDto>  Investments { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
