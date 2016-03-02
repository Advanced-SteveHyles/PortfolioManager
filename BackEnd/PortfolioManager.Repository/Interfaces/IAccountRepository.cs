using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
{
    public interface IAccountRepository
    {
        RepositoryActionResult<Account> InsertAccount(Account entityAccount);
        Account GetAccountWithInvestments(int id);
        Account GetAccount(int id);

        void IncreaseAccountBalance(int accountId, decimal amount);
        void DecreaseAccountBalance(int accountId, decimal amount);

        void IncreaseValuation(int accountId, decimal valuation);
        void DecreaseValuation(int accountId, decimal valuation);
        IEnumerable<Account> GetAccounts();
        void SetValuation(int accountId, decimal valuation);
    }
}