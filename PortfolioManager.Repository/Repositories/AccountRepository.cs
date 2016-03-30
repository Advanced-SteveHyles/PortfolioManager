using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace Portfolio.BackEnd.Repository.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {        
        public AccountRepository(string connection) : base(new PortfolioManagerContext (connection))
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

        public Account GetAccountByAccountId(int accountId)
        {
            var account = _context.Accounts.SingleOrDefault(p => p.AccountId == accountId);
            return account;
        }

        public Account GetAccountWithInvestments(int accountId)
        {
            var account = _context.Accounts.Include("Investments").SingleOrDefault(p => p.AccountId == accountId);
            return account;
        }

        public void AdjustAccountBalance(int accountId, decimal amount)
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
            var account = GetAccountByAccountId(accountId);
            account.Valuation -= valuation;
            _context.SaveChanges();
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _context.Accounts;
        }

        public IEnumerable<Account> GetAccountsForPortfolio(int portfolioId)
        {
            return _context.Accounts.Where(p => p.PortfolioId == portfolioId).ToList();
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation = valuation;
            _context.SaveChanges();
        }
    }
}
