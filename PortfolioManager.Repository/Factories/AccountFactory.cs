using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Factories
{
    public class AccountFactory
    {
        public static Account CreateAccount(AccountRequest account)
        {
            return new Account()
            {
                PortfolioId = account.PortfolioId,
                Name = account.Name,
                Investments = new List<AccountInvestmentMap>(),
                Cash = 0,
                Valuation = 0,
                Type = "Not Set"
            };
        }
    }
}