using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(PortfolioManagerContext context) : base(context)
        {
        }

        public RepositoryActionResult<Account> InsertAccount(Account entityAccount)
        {
            try
            {
                _context.Accounts.Add(entityAccount);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Account>(entityAccount, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Account>(entityAccount, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Account>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public Account GetAccount(int id)
        {
            var account = _context.Accounts.SingleOrDefault(p => p.AccountId == id);
            return account;
        }

        public Account GetAccountWithInvestments(int id)
        {
            var account = _context.Accounts.Include("Investments").SingleOrDefault(p => p.AccountId == id);
            return account;
        }

        public void IncreaseAccountBalance(int accountId, decimal amount)
        {
            var account = _context.Accounts.Single(a => a.AccountId == accountId);
            account.Cash += amount;
            _context.SaveChanges();

        }

        public void DecreaseAccountBalance(int accountId, decimal amount)
        {
            var account = _context.Accounts.Single(a => a.AccountId == accountId);
            account.Cash -= amount;
            _context.SaveChanges();
        }

        public void IncreaseValuation(int accountId, decimal valuation)
        {
            var account = _context.Accounts.Single(a => a.AccountId == accountId);
            account.Valuation += valuation;
            _context.SaveChanges();
        }

        public void DecreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccount(accountId);
            account.Valuation -= valuation;
            _context.SaveChanges();
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _context.Accounts;
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccount(accountId);
            account.Valuation = valuation;
            _context.SaveChanges();
        }
    }
}
