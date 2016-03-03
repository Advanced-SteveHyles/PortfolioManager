using System;
using System.Linq;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository.Repositories
{
    public class AccountInvestmentMapRepository : BaseRepository, IAccountInvestmentMapRepository
    {
        public AccountInvestmentMapRepository(PortfolioManagerContext context) : base(context){}

        public RepositoryActionResult<AccountInvestmentMap> InsertAccountInvestmentMap(AccountInvestmentMap entityAccountInvestmentMap)
        {
            try
            {
                _context.AccountInvestmentMaps.Add(entityAccountInvestmentMap);
                var result = _context.SaveChanges();
                return result > 0
                    ? new RepositoryActionResult<AccountInvestmentMap>(entityAccountInvestmentMap, RepositoryActionStatus.Created)
                    : new RepositoryActionResult<AccountInvestmentMap>(entityAccountInvestmentMap, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<AccountInvestmentMap>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByInvestmentId(int investmentId)
        {
            var accountInvestmentMaps = _context.AccountInvestmentMaps
                .Where(accountInvestmentMap => accountInvestmentMap.InvestmentId == investmentId);

            return accountInvestmentMaps;               
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMaps()
        {
            return _context.AccountInvestmentMaps;
        }

        public AccountInvestmentMap GetAccountInvestmentMap(int accountInvestmentMapId)
        {
            return _context.AccountInvestmentMaps.SingleOrDefault(aiv => aiv.AccountInvestmentMapId == accountInvestmentMapId);
        }

        public void UpdateAccountInvestmentMap(AccountInvestmentMap investmentMap)
        {
            var accountInvestmentMap = GetAccountInvestmentMap(investmentMap.AccountInvestmentMapId);
            accountInvestmentMap.Quantity = investmentMap.Quantity;
            accountInvestmentMap.Valuation = investmentMap.Valuation;
            _context.SaveChanges();
        }
    }
}
