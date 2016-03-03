namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IPortfolioRepository
    {     
        Entities.Portfolio GetPortfolio(int id);
        Entities.Portfolio GetPortfolioWithAccounts(int id);
        System.Linq.IQueryable<Entities.Portfolio> GetPortfolios();

        RepositoryActionResult<Entities.Portfolio> InsertPortfolio(Entities.Portfolio entityPortfolio);
    }
}
