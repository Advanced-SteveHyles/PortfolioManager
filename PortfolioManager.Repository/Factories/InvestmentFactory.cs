using PortfolioManager.DTO;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Factories
{
    public class InvestmentFactory
    {
        public Investment CreateInvestment(InvestmentRequest investmentRequest)
        {
            return new Investment()
            {
                Name = investmentRequest.Name,
                Symbol = investmentRequest.Symbol,
                Type = investmentRequest.Type,
                Class = investmentRequest.Class,
                IncomeType = investmentRequest.IncomeType,
                MarketIndex = investmentRequest.MarketIndex
            };
        }
    }
}