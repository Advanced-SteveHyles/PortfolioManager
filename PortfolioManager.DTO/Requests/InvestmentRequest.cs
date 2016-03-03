namespace PortfolioManager.DTO
{
    public class InvestmentRequest
    {
        public int InvestmentId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
        public string IncomeType { get; set; }
        public string MarketIndex { get; set; }
    }
}