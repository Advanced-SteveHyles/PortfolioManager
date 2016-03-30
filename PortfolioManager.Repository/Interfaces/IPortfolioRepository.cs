using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IPortfolioRepository
    {     
        Entities.Portfolio GetPortfolio(int id);
        Entities.Portfolio GetPortfolioWithAccounts(int id);
        System.Linq.IQueryable<Entities.Portfolio> GetPortfolios();

        PortfolioValuation GetPortfolioValuation(int portfolioId);

        RepositoryActionResult<Entities.Portfolio> InsertPortfolio(Entities.Portfolio entityPortfolio);
        RepositoryActionResult<PortfolioValuation> UpdatePortfolioValuation(PortfolioValuation valuation);
    }
}
