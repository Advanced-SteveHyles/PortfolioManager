using System;
using System.Collections;
using System.Collections.Generic;
using PortfolioManager.DTO;
using PortfolioManager.DTO.DTOs;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.DTO.Transactions;
using PortfolioManager.Repository.Entities;

namespace Interfaces
{
    public interface IAccountHandlers
    {
        void IncreaseAccountBalance(int accountId, decimal amount);
        void DecreaseAccountBalance(int accountId, decimal amount);
        void IncreaseValuation(int accountId, decimal mapValue);
        void DecreaseValuation(int accountId, decimal mapValue);
        void SetValuation(int accountId, decimal valuation);
        Account GetAccount(int accountId);
        IEnumerable<Account> GetAccounts();
    }

    public interface ICashTransactionHandler
    {
        void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest);
        void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest);
        void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest);
        void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest);
    }

    public interface IFundTransactionHandler
    {
        void StoreFundTransaction(InvestmentBuyRequest request);
        void StoreFundTransaction(InvestmentCorporateActionRequest request);
    }

    public interface IAccountInvestmentMapProcessor
    {
        void ChangeQuantity(int investmentMapId, decimal quantity);
        decimal RevalueMap(int investmentMapId, decimal? currentSellPrice);
        AccountInvestmentMapDto GetAccountInvestmentMap(int investmentMapId);
        List<AccountInvestmentMap> GetMapsByInvestmentId(int investmentId);
        List<AccountInvestmentMap> GetMapsByAccountId(int accountId);
    }

    public interface IPriceHistoryHandler
{
        void StorePriceHistory(PriceHistoryRequest priceHistoryRequest, DateTime recordedDate);
        decimal? GetInvestmentSellPrice(int investmentId, DateTime valuationDate);
        decimal? GetInvestmentBuyPrice(int investmentId, DateTime valuationDate);
}

    public interface IInvestmentHandler
    {
        InvestmentDto GetInvestment(int investmentId);
        IEnumerable<InvestmentDto> GetInvestments();
    }

}
