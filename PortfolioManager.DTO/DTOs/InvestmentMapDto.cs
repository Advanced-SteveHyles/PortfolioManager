namespace PortfolioManager.DTO.DTOs
{
    public class AccountInvestmentMapDto
    {
        public int AccountInvestmentMapId { get; set; }

        public int AccountId { get; set; }
        public int InvestmentId { get; set; }
        
        public string InvestmentName { get; set; }

        public decimal Quantity { get; set; }
        public decimal Valuation { get; set; }
    }
}