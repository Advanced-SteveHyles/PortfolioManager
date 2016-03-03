using System.Collections.Generic;
using System.Linq;
using PortfolioManager.DTO.DTOs;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
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