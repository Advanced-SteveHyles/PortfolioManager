using System;
using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace Portfolio.BackEnd.Repository.Repositories
{
    public class PortfolioRepository : BaseRepository, IPortfolioRepository
    {
        public PortfolioRepository(string connection) : base(new PortfolioManagerContext (connection))
        {
        }

        public IQueryable<Entities.Portfolio> GetPortfolios()
        {
            return _context.Portfolios;
        }

        public Entities.Portfolio GetPortfolio(int id)
        {
            var portfolio = _context.Portfolios.SingleOrDefault(p => p.PortfolioId == id);
            return portfolio;
        }

        public Entities.Portfolio GetPortfolioWithAccounts(int id)
        {
            var portfolio = _context.Portfolios.Include("Accounts").SingleOrDefault(p => p.PortfolioId == id);

            return portfolio;
        }

        public RepositoryActionResult<Entities.Portfolio> InsertPortfolio(Entities.Portfolio entityPortfolio)
        {
            try
            {
                _context.Portfolios.Add(entityPortfolio);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Entities.Portfolio>(entityPortfolio, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Entities.Portfolio>(entityPortfolio, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Entities.Portfolio>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public PortfolioValuation GetPortfolioValuation(int portfolioId)
        {
            var portfolioValuation = _context.PortfolioValuations.SingleOrDefault(p => p.PortfolioId == portfolioId);

            return portfolioValuation;
        }
        

        public RepositoryActionResult<PortfolioValuation> UpdatePortfolioValuation(Entities.PortfolioValuation valuation)
        {
            try
            {
                _context.PortfolioValuations.Add(valuation);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Entities.PortfolioValuation>(valuation, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Entities.PortfolioValuation>(valuation, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Entities.PortfolioValuation>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}