using System.Collections.Generic;
using Interfaces;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
{
    public class AccountHandler : IAccountHandler
    {
        private readonly IAccountRepository _repository;

        public AccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public void AdjustAccountBalance(int accountId, decimal amount)
        {
            _repository.AdjustAccountBalance(accountId, amount);
        }
        
        public void IncreaseValuation(int accountId, decimal mapValue)
        {
            _repository.IncreaseValuation(accountId, mapValue);
        }

        public void DecreaseValuation(int accountId, decimal mapValue)
        {
            _repository.DecreaseValuation(accountId, mapValue);
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            _repository.SetValuation(accountId, valuation);
        }


        public Account GetAccount(int accountId)
        {
            return _repository.GetAccountByAccountId(accountId);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _repository.GetAccounts();
        }
    }
}