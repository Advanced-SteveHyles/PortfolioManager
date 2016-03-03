using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Factories
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