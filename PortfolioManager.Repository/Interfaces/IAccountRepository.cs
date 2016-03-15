using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IAccountRepository
    {
        RepositoryActionResult<Account> InsertAccount(Account entityAccount);
        Account GetAccountWithInvestments(int id);
        Account GetAccountByAccountId(int accountId);

        void IncreaseAccountBalance(int accountId, decimal amount);
        void DecreaseAccountBalance(int accountId, decimal amount);

        void IncreaseValuation(int accountId, decimal valuation);
        void DecreaseValuation(int accountId, decimal valuation);
        IEnumerable<Account> GetAccounts();
        void SetValuation(int accountId, decimal valuation);
    }
}