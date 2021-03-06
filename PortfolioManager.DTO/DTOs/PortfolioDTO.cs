﻿using System.Collections.Generic;

namespace Portfolio.Common.DTO.DTOs
{
    public class PortfolioDto
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public ICollection<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }

    public class PortfolioValuationDto
    {
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
