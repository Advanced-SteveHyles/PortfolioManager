using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
{
    public interface IPortfolioRepository
    {     
        Portfolio GetPortfolio(int id);
        Portfolio GetPortfolioWithAccounts(int id);
        System.Linq.IQueryable<Portfolio> GetPortfolios();

        RepositoryActionResult<Portfolio> InsertPortfolio(Portfolio entityPortfolio);
    }
}
