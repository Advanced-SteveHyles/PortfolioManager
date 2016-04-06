using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.FakeRepositories
{
    public class FakePortfolioRepository
        : IPortfolioRepository         
    {
        private readonly List<PortfolioValuation> _portfolioValuations;

        public FakePortfolioRepository()
        {
            _portfolioValuations = new List<PortfolioValuation>();
        }

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

        public RepositoryActionResult<PortfolioValuation> UpdatePortfolioValuation(PortfolioValuation valuation)
        {
            _portfolioValuations.Add(valuation);
            return new RepositoryActionResult<PortfolioValuation>(valuation, RepositoryActionStatus.Ok);
        }

        public PortfolioValuation GetPortfolioValuation(int portfolioId)
        {
            return _portfolioValuations.Single(ph => ph.PortfolioId == portfolioId);
        }
    }
}