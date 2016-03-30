using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Factories
{
    public class PortfolioFactory
    {
        public Entities.Portfolio CreatePortfolio(PortfolioRequest portfolio)
        {
            return new Entities.Portfolio()
            {
                Name = portfolio.Name,
                Accounts = new List<Account>()
            };
        }

        public PortfolioValuation CreatePortfolioValuation(PortfolioRevaluationRequest request)
        {
            return new Entities.PortfolioValuation()
            {
                PortfolioId = request.PortfolioId                
            };
        }
    }
}
