using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace BusinessLogicTests.Fakes
{
    public class FakeRepository        :
        IInvestmentRepository
        , IAccountRepository
        , IAccountInvestmentMapRepository
        , IFundTransactionRepository
        , IPriceHistoryRepository

    {
        private readonly FakeData _fakeData;
                        
        public FakeRepository(FakeData fakeData)
        {
            _fakeData = fakeData;                                   
        }
        
        public RepositoryActionResult<Account> InsertAccount(Account entityAccount)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountWithInvestments(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByAccountId(int id)
        {
            return _fakeData.Accounts().Single(a => a.AccountId == id);
        }

        public void AdjustAccountBalance(int accountId, decimal amount)
        {
            GetAccountByAccountId(accountId).Cash += amount;
        }

        public void DecreaseAccountBalance(int accountId, decimal amount)
        {
            GetAccountByAccountId(accountId).Cash -= amount;
        }

        public void IncreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation += valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }

        public void DecreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation -= valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _fakeData.Accounts();
        }

        public IEnumerable<Account> GetAccountsForPortfolio(int portfolioId)
        {
            return _fakeData.Accounts().Where(acc => acc.PortfolioId == portfolioId).ToList();
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation = valuation;
            _fakeData.Accounts().RemoveAll(acc => acc.AccountId == accountId);
            _fakeData.Accounts().Add(account);
        }


        

        public RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Investment> GetInvestments()
        {
            return _fakeData.InvestmentMaps().Select(inv => new Investment()
            {
                InvestmentId = inv.InvestmentId,
            }).AsQueryable();
        }

        public Investment GetInvestment(int investmentId)
        {
            return _fakeData.Investments().Single(inv => inv.InvestmentId == investmentId);
        }
        
        public AccountInvestmentMap GetAccountInvestmentMap(int accountInvestmentMapId)
        {
            var accountInvestmentMapDto =
           _fakeData.InvestmentMaps().SingleOrDefault(i => i.AccountInvestmentMapId == accountInvestmentMapId);

            if (accountInvestmentMapDto != null)
                return new AccountInvestmentMap()
                {
                    AccountId = accountInvestmentMapDto.AccountId,
                    AccountInvestmentMapId = accountInvestmentMapDto.AccountInvestmentMapId,
                    InvestmentId = accountInvestmentMapDto.InvestmentId,
                    Quantity = accountInvestmentMapDto.Quantity,
                    Valuation = accountInvestmentMapDto.Valuation
                };

            return null;
        }

        public void UpdateAccountInvestmentMap(AccountInvestmentMap investmentMap)
        {
            var map = GetAccountInvestmentMap(investmentMap.AccountInvestmentMapId);
            map.Valuation = investmentMap.Valuation;
            map.Quantity = investmentMap.Quantity;

            _fakeData.InvestmentMaps().RemoveAll(f => f.AccountInvestmentMapId == map.AccountInvestmentMapId);
            _fakeData.InvestmentMaps().Add(map);
        }

        public RepositoryActionResult<AccountInvestmentMap> InsertAccountInvestmentMap(AccountInvestmentMap entityAccountInvestmentMap)
        {
            var map = new AccountInvestmentMap()
            {
                AccountInvestmentMapId = entityAccountInvestmentMap.AccountInvestmentMapId,
                AccountId = entityAccountInvestmentMap.AccountId,
                InvestmentId = entityAccountInvestmentMap.InvestmentId,
                Quantity = entityAccountInvestmentMap.Quantity,
                Valuation = entityAccountInvestmentMap.Valuation
            };

            _fakeData.InvestmentMaps().Add(map);

            return new RepositoryActionResult<AccountInvestmentMap>(map, RepositoryActionStatus.Created);
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByInvestmentId(int investmentId)
        {
            return _fakeData.InvestmentMaps().Where(inv => inv.InvestmentId == investmentId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByAccountId(int accountId)
        {
            return _fakeData.InvestmentMaps().Where(inv => inv.AccountId == accountId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMaps()
        {
            return _fakeData.InvestmentMaps().Select(map => new AccountInvestmentMap()
            {
                AccountId = map.AccountId,
                Valuation = map.Valuation
            }).AsQueryable();
        }

        public FundTransaction GetFundTransaction(int fundTransactionId)
        {            
            return _fakeData.FundTransactions().Single(t => t.FundTransactionId == fundTransactionId);
        }

        private int _nextFundTransactionId;
        public RepositoryActionResult<FundTransaction> InsertFundTransaction(CreateFundTransactionRequest request)
        {
            _nextFundTransactionId++;
            var dummyFundTransaction = new FundTransaction()
            {
                FundTransactionId = _nextFundTransactionId,

                InvestmentMapId = request.InvestmentMapId,
                TransactionType = request.TransactionType,
                TransactionDate = request.TransactionDate,
                SettlementDate = request.SettlementDate,
                Source = request.Source,
                Quantity = request.Quantity,
                SellPrice = request.SellPrice,
                BuyPrice = request.BuyPrice,
                Charges = request.Charges,
                TransactionValue = request.TransactionValue,
                LinkedTransactionType = request.LinkedTransactionType,
                LinkedTransaction = request.LinkedTransaction
            };

            _fakeData.FundTransactions().Add(dummyFundTransaction);

            return new RepositoryActionResult<FundTransaction>(dummyFundTransaction, RepositoryActionStatus.Ok);
        }


        public IQueryable<PriceHistory> GetInvestmentSellPrices(int investmentId)
        {
            return _fakeData.PriceHistories().Where(ph => ph.InvestmentId == investmentId).AsQueryable();
        }

        public IQueryable<PriceHistory> GetInvestmentBuyPrices(int investmentId)
        {
            return _fakeData.PriceHistories().Where(ph => ph.InvestmentId == investmentId).AsQueryable();
        }

        private int _priceHistoryId;
        public RepositoryActionResult<PriceHistory> InsertPriceHistory(int investmentId, DateTime valuationDate, decimal? buyPrice, decimal? sellPrice, DateTime recordedDate)
        {
            _priceHistoryId++;
               var priceHistory = new PriceHistory
            {
                PriceHistoryId = _priceHistoryId,
                InvestmentId = investmentId,
                ValuationDate = valuationDate,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                RecordedDate = recordedDate
            };

            _fakeData.PriceHistories().Add(priceHistory);

            return null;
        }

        public void SetInvestmentClass(int fakeInvestmentId, string investmentClass)
        {
            var investment = _fakeData
                .Investments()
                .First(i => i.InvestmentId == fakeInvestmentId);

            _fakeData.Investments().Remove(investment);
            investment.Class = investmentClass;
            _fakeData.Investments().Add(investment);
        }

        public void SetInvestmentIncome(int fakeInvestmentId,  string investmentIncomeType)
        {
            var investment = _fakeData
                .Investments()
                .First(i => i.InvestmentId == fakeInvestmentId);

            _fakeData.Investments().Remove(investment);
            investment.IncomeType = investmentIncomeType;
            _fakeData.Investments().Add(investment);
        }

        public List<AccountInvestmentMap> GetAllAccountInvestmentMaps()
        {
            return _fakeData.InvestmentMaps();
        }
        
       
    }
}