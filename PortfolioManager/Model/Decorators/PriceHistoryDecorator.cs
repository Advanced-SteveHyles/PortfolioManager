using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.Common.DTO.DTOs;

namespace PortfolioManager.Model.Decorators
{
    public class PriceHistoryDecorator
    {
        private readonly InvestmentDto _investment;
        public int InvestmentId => _investment.InvestmentId;

        public PriceHistoryDecorator(InvestmentDto investment)
        {
            _investment = investment;
        }

        public string InvestmentName => _investment.Name;

        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
    }
}
