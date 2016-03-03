using System.Linq;
using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IAccountInvestmentMapRepository
    {
        AccountInvestmentMap GetAccountInvestmentMap(int accountInvestmentMapId);
        void UpdateAccountInvestmentMap(AccountInvestmentMap investmentMap);
        RepositoryActionResult<AccountInvestmentMap> InsertAccountInvestmentMap(AccountInvestmentMap entityAccountInvestmentMap);
        IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByInvestmentId(int investmentId);
        IQueryable<AccountInvestmentMap> GetAccountInvestmentMaps();
    }
}