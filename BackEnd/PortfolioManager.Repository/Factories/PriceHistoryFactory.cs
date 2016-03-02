using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Factories
{
    public class PriceHistoryFactory
    {
        public PriceHistory CreatePriceHistory(PriceHistoryRequest request)
        {
            return new PriceHistory()
            {
                InvestmentId = request.InvestmentId,
                BuyPrice = request.BuyPrice,
                SellPrice = request.SellPrice,
                ValuationDate = request.ValuationDate
            };
        }
    }
}