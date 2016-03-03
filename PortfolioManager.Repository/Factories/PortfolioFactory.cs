using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Factories
{
    public class PortfolioFactory
    {
        public Portfolio CreatePortfolio(PortfolioRequest portfolio)
        {
            return new Portfolio()
            {
                Name = portfolio.Name,
                Accounts = new List<Account>()
            };
        }
    }
}
