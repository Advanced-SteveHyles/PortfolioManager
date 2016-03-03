using System;
using System.Linq;
using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IPriceHistoryRepository
    {
        IQueryable<PriceHistory> GetInvestmentSellPrices(int investmentId);
        IQueryable<PriceHistory> GetInvestmentBuyPrices(int investmentId);
        RepositoryActionResult<PriceHistory> InsertPriceHistory(int investmentId, DateTime valuationDate, decimal? buyPrice, decimal? sellPrice, DateTime recordedDate);
    }
}