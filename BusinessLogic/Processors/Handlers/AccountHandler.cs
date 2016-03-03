using System.Collections.Generic;
using Interfaces;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogic.Processors.Handlers
{
    public class AccountHandler : IAccountHandlers
    {
        private readonly IAccountRepository _repository;

        public AccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public void IncreaseAccountBalance(int accountId, decimal amount)
        {
            _repository.IncreaseAccountBalance(accountId, amount);
        }

        public void DecreaseAccountBalance(int accountId, decimal amount)
        {
            _repository.DecreaseAccountBalance(accountId, amount);
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
            return _repository.GetAccount(accountId);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _repository.GetAccounts();
        }
    }
}