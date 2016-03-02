using System;
using System.Linq;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository.Repositories
{
    public class PriceHistoryRepository : BaseRepository, IPriceHistoryRepository
    {
        public PriceHistoryRepository(PortfolioManagerContext context) : base(context) { }

        public IQueryable<PriceHistory> GetInvestmentSellPrices(int investmentId)
        {
            return _context.PriceHistories
                .Where(ph => ph.InvestmentId == investmentId);
        }

        public IQueryable<PriceHistory> GetInvestmentBuyPrices(int investmentId)
        {
            return _context.PriceHistories
                .Where(ph => ph.InvestmentId == investmentId);
        }

        public RepositoryActionResult<PriceHistory> InsertPriceHistory(int investmentId, DateTime valuationDate, decimal? buyPrice, decimal? sellPrice, DateTime recordedDate)
        {

            try
            {
                var entityPriceHistory = new PriceHistory()
                {
                    InvestmentId = investmentId,
                    BuyPrice =buyPrice,
                    SellPrice = sellPrice,
                    ValuationDate = valuationDate,
                    RecordedDate = recordedDate
                };

                _context.PriceHistories.Add(entityPriceHistory);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<PriceHistory>(entityPriceHistory, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<PriceHistory>(entityPriceHistory, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<PriceHistory>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}