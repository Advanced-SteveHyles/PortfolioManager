using System;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.FakeRepositories
{
    public class FakePortfolioRepository
        : IPortfolioRepository         
    {
        
        public IQueryable<Portfolio.BackEnd.Repository.Entities.Portfolio> GetPortfolios()
        {
            throw new NotImplementedException();
        }

        public Portfolio.BackEnd.Repository.Entities.Portfolio GetPortfolio(int id)
        {
            throw new NotImplementedException();
        }

        public Portfolio.BackEnd.Repository.Entities.Portfolio GetPortfolioWithAccounts(int id)
        {
            throw new NotImplementedException();
        }

        public RepositoryActionResult<Portfolio.BackEnd.Repository.Entities.Portfolio> InsertPortfolio(Portfolio.BackEnd.Repository.Entities.Portfolio entityPortfolio)
        {
            throw new NotImplementedException();
        }
    }
}