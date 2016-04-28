using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.Fakes
{
    public class FakeAccountRepository : IAccountRepository
    {

        private readonly FakeData _fakeData;

        public FakeAccountRepository(FakeData fakeData)
        {
            _fakeData = fakeData;
        }

        public RepositoryActionResult<Account> InsertAccount(Account entityAccount)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountWithInvestments(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByAccountId(int id)
        {
            return _fakeData.Accounts().Single(a => a.AccountId == id);
        }

        public void AdjustAccountBalance(int accountId, decimal amount)
        {
            GetAccountByAccountId(accountId).Cash += amount;
        }

        public void DecreaseAccountBalance(int accountId, decimal amount)
        {
            GetAccountByAccountId(accountId).Cash -= amount;
        }

        public void IncreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation += valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }

        public void DecreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation -= valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _fakeData.Accounts();
        }

        public IEnumerable<Account> GetAccountsForPortfolio(int portfolioId)
        {
            return _fakeData.Accounts().Where(acc => acc.PortfolioId == portfolioId).ToList();
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation = valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }


   
    }
}