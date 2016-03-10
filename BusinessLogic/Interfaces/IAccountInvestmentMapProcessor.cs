using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.DTOs;

namespace Interfaces
{
    public interface IAccountInvestmentMapProcessor
    {
        void ChangeQuantity(int investmentMapId, decimal quantity);
        decimal RevalueMap(int investmentMapId, decimal? currentSellPrice);
        
        List<AccountInvestmentMap> GetMapsByInvestmentId(int investmentId);
        List<AccountInvestmentMap> GetMapsByAccountId(int accountId);

        AccountInvestmentMap GetAccountInvestmentMap(int investmentMapId);    
    }
}