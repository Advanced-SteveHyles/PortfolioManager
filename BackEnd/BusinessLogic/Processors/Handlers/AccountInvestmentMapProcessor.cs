using System.Collections.Generic;
using System.Linq;
using Interfaces;
using PortfolioManager.DTO.DTOs;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogic.Processors.Handlers
{
    public class AccountInvestmentMapProcessor : IAccountInvestmentMapProcessor
    {
        private readonly IAccountInvestmentMapRepository  _accountInvestmentMapRepository;

        public AccountInvestmentMapProcessor(
            IAccountInvestmentMapRepository accountInvestmentMapRepository)
        {
            _accountInvestmentMapRepository = accountInvestmentMapRepository;
        }        

        public void ChangeQuantity(int investmentMapId, decimal quantity)
        {            
            var investmentMap = _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId);
            investmentMap.Quantity += quantity;            
            _accountInvestmentMapRepository.UpdateAccountInvestmentMap(investmentMap);        
        }

        public decimal RevalueMap(int investmentMapId, decimal? currentSellPrice)
        {
            var investmentMap = _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId);

            var valuation = investmentMap.Quantity*currentSellPrice;
            investmentMap.Valuation = valuation??0 ;        
            _accountInvestmentMapRepository.UpdateAccountInvestmentMap(investmentMap);

            return valuation ??0;
        }

        public AccountInvestmentMapDto GetAccountInvestmentMap(int investmentMapId)
        {
            return _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId).MapToDto();
        }

        public List<AccountInvestmentMap> GetMapsByInvestmentId(int investmentId)
        {
            return _accountInvestmentMapRepository
                    .GetAccountInvestmentMapsByInvestmentId(investmentId)
                    .ToList();
        }

        public List<AccountInvestmentMap> GetMapsByAccountId(int accountId)
        {
            return _accountInvestmentMapRepository.GetAccountInvestmentMaps()
                .Where(map => map.AccountId == accountId)
                .ToList();
        }
    }
}