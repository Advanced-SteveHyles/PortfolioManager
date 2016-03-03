using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.Repository.Factories
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