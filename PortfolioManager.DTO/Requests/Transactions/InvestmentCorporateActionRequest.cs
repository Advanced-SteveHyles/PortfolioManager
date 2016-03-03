using System;

namespace PortfolioManager.DTO.Requests.Transactions
{
    public class InvestmentCorporateActionRequest
    {
        public int InvestmentMapId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}