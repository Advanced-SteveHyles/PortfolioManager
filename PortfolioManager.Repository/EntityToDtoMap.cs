using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace Portfolio.BackEnd.Repository
{
    public static class EntityToDtoMap
    {
        public static PortfolioDto MapToDto(this Entities.Portfolio entity)
        {
            return new PortfolioDto
            {
                PortfolioId = entity.PortfolioId,
                Name = entity.Name,

                //        Accounts = portfolioEnt.Accounts.Select(e => CreateAccount(e, false)).ToList()
            };
        }
        
        public static AccountDto MapToDto(this Account entity)
        {
            return new AccountDto()
            {
                AccountId = entity.AccountId,
                Name = entity.Name,
                Type = entity.Type,
                Cash = entity.Cash,
                Valuation = entity.Valuation,
                AccountBalance = entity.Cash,
                PortfolioId = entity.PortfolioId

                //  Accounts = portfolioEnt.Accounts.Select(e => CreateAccount(e, false)).ToList()
            };
        }



      public static TransactionDto MapToDto(object entity)
        {
            throw new System.NotImplementedException();
        }

      public static InvestmentDto MapToDto(this Investment entity)
      {
        return new InvestmentDto
        {
          InvestmentId = entity.InvestmentId,
          Name = entity.Name,
          Symbol = entity.Symbol,
          Type = entity.Type,
          Class = entity.Class,
          IncomeType = entity.IncomeType,
          MarketIndex = entity.MarketIndex
        };
      }

      public static AccountInvestmentMapDto MapToDto(this AccountInvestmentMap accountInvestmentMap)
        {
            return new AccountInvestmentMapDto
            {
                AccountInvestmentMapId = accountInvestmentMap.AccountInvestmentMapId,
                AccountId = accountInvestmentMap.AccountId,
                InvestmentId = accountInvestmentMap.InvestmentId,                
                Quantity = accountInvestmentMap.Quantity,                
                Valuation = accountInvestmentMap.Valuation ?? 0,                                
            };
        }
    }
}