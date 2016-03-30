using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IAccountRepository
    {
        RepositoryActionResult<Account> InsertAccount(Account entityAccount);
        Account GetAccountWithInvestments(int id);
        Account GetAccountByAccountId(int accountId);

        void AdjustAccountBalance(int accountId, decimal amount);
        
        void IncreaseValuation(int accountId, decimal valuation);
        void DecreaseValuation(int accountId, decimal valuation);
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccountsForPortfolio(int portfolioId);
        void SetValuation(int accountId, decimal valuation);
    }
}