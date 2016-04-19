using System;
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
                Name = entity.Name

                //        Accounts = portfolioEnt.Accounts.Select(e => CreateAccount(e, false)).ToList()
            };
        }

        public static PortfolioValuationDto MapToDto(this PortfolioValuation entity)
        {
            return new PortfolioValuationDto
            {
                PortfolioValuationId = entity.PortfolioValuationId,
                PortfolioId = entity.PortfolioId,
                PropertyValue = entity.PropertyValue,
                PropertyRatio = entity.PropertyRatio,
                CashValue = entity.CashValue,
                CashRatio = entity.CashRatio,
                BondValue = entity.BondValue,
                BondRatio = entity.BondRatio,
                EquityValue = entity.EquityValue,
                EquityRatio = entity.EquityValue
            };
        }


        public static AccountDto MapToDto(this Account entity)
        {
            return new AccountDto
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

        public static AccountInvestmentMapDto MapToDto(this AccountInvestmentMap accountInvestmentMap,
            string investmentName
            )
        {
            return new AccountInvestmentMapDto
            {
                AccountInvestmentMapId = accountInvestmentMap.AccountInvestmentMapId,
                AccountId = accountInvestmentMap.AccountId,
                InvestmentId = accountInvestmentMap.InvestmentId,
                Quantity = accountInvestmentMap.Quantity,
                Valuation = accountInvestmentMap.Valuation ?? 0,
                InvestmentName = investmentName
            };
        }

        public static CashTransactionDto MapToDto(this CashTransaction cashTransaction)
        {
            return new CashTransactionDto
            {
                CashTransactionId = cashTransaction.CashTransactionId,
                AccountId = cashTransaction.AccountId,
                TransactionType = cashTransaction.TransactionType,
                TransactionDate = cashTransaction.TransactionDate,
                Source = cashTransaction.Source,
                TransactionValue = cashTransaction.TransactionValue,
                IsTaxRefund = cashTransaction.IsTaxRefund
            };
        }
    }
}