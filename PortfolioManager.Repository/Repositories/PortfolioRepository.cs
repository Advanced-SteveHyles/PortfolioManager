using System;
using System.Linq;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository
{
    public class PortfolioRepository : BaseRepository, IPortfolioRepository
    {
        public PortfolioRepository(PortfolioManagerContext context) : base(context)
        {
        }

        public IQueryable<Portfolio> GetPortfolios()
        {
            return _context.Portfolios;
        }

        public Portfolio GetPortfolio(int id)
        {
            var portfolio = _context.Portfolios.SingleOrDefault(p => p.PortfolioId == id);
            return portfolio;
        }

        public Portfolio GetPortfolioWithAccounts(int id)
        {
            var portfolio = _context.Portfolios.Include("Accounts").SingleOrDefault(p => p.PortfolioId == id);

            return portfolio;
        }

        public RepositoryActionResult<Portfolio> InsertPortfolio(Portfolio entityPortfolio)
        {
            try
            {
                _context.Portfolios.Add(entityPortfolio);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Portfolio>(entityPortfolio, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Portfolio>(entityPortfolio, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Portfolio>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}