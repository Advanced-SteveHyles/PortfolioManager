using System;
using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Factories
{
    public  class InvestmentMapFactory
    {
        public static AccountInvestmentMap CreateAccountInvestmenMap(AccountInvestmentMapRequest investmentMapRequest)
        {
            return new AccountInvestmentMap
            {
                AccountId = investmentMapRequest.AccountId,
                InvestmentId = investmentMapRequest.InvestmentId,              
                Quantity = 0,
                Valuation = 0                
            };
        }
    }
}