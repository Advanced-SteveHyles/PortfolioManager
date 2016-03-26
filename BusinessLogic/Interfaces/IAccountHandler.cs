using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;

namespace Interfaces
{
    public interface IAccountHandler
    {
        void AdjustAccountBalance(int accountId, decimal amount);

        void IncreaseValuation(int accountId, decimal mapValue);
        void DecreaseValuation(int accountId, decimal mapValue);
        void SetValuation(int accountId, decimal valuation);
        Account GetAccount(int accountId);
        IEnumerable<Account> GetAccounts();
    }
}
