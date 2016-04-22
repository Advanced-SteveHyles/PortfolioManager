using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace BusinessLogicTests.Fakes
{
    public class FakeRepository        :
        IInvestmentRepository
        , IAccountRepository
        , ICashTransactionRepository
        , IAccountInvestmentMapRepository
        , IFundTransactionRepository
        , IPriceHistoryRepository

    {
        private readonly IFakeData _fakeData;
        private readonly List<CashTransaction> _dummyCashTransactions;
        private readonly List<FundTransaction> _dummyFundTransactions;
        private readonly List<PriceHistory> _dummyPriceHistoryList;
        private readonly List<AccountInvestmentMap> _investmentMaps;        

        readonly List<Account> _accounts;

        public FakeRepository(IFakeData fakeData)
        {
            _fakeData = fakeData;
            _dummyCashTransactions = new List<CashTransaction>();
            _dummyFundTransactions = new List<FundTransaction>();
            _dummyPriceHistoryList = new List<PriceHistory>();
            _investmentMaps = fakeData.FakePopulatedInvestmentMap();            
            _accounts = fakeData.FakeAccountData();            
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
            return _accounts.Single(a => a.AccountId == id);
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
            _accounts.RemoveAll(acc => acc.AccountId == accountId);
            _accounts.Add(account);
        }

        public void DecreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation -= valuation;
            _accounts.RemoveAll(acc => acc.AccountId == accountId);
            _accounts.Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accounts;
        }

        public IEnumerable<Account> GetAccountsForPortfolio(int portfolioId)
        {
            return _accounts.Where(acc => acc.PortfolioId == portfolioId).ToList();
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccountByAccountId(accountId);
            account.Valuation = valuation;
            _accounts.RemoveAll(acc => acc.AccountId == accountId);
            _accounts.Add(account);
        }


        public IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId)
        {
            return _dummyCashTransactions.Where(ct => ct.AccountId == accountId).AsQueryable();
        }

        public RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Investment> GetInvestments()
        {
            return _investmentMaps.Select(inv => new Investment()
            {
                InvestmentId = inv.InvestmentId,
            }).AsQueryable();
        }

        public Investment GetInvestment(int investmentId)
        {
            return _fakeData.FakePopulatedInvestments().Single(inv => inv.InvestmentId == investmentId);
        }


        private int _nextCashTransactionId;
        public RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request)
        {
            _nextCashTransactionId++;
            var cashTransaction = new CashTransaction()
            {
                CashTransactionId = _nextCashTransactionId,
                AccountId = request.AccountId,
                TransactionDate = request.TransactionDate,
                TransactionValue = request.TransactionValue,
                Source = request.Source,
                IsTaxRefund = request.IsTaxRefund,
                TransactionType = request.TransactionType,
                LinkedTransactionType = request.LinkedTransactionType,
                LinkedTransaction = request.LinkedTransaction
            };
            _dummyCashTransactions.Add(
                cashTransaction
                );
            
            return null;
        }

        public RepositoryActionResult<CashTransaction> ApplyCheckpoint(CashCheckpoint cashCheckpoint, int transactionId)
        {
            var transaction =GetCashTransaction(transactionId);
            _dummyCashTransactions.Remove(transaction);
            transaction.CheckpointId = cashCheckpoint.CashCheckpointId;
            _dummyCashTransactions.Add(transaction);

            return new RepositoryActionResult<CashTransaction>(transaction, RepositoryActionStatus.Updated);
        }

        public AccountInvestmentMap GetAccountInvestmentMap(int accountInvestmentMapId)
        {
            var accountInvestmentMapDto =
                _investmentMaps.SingleOrDefault(i => i.AccountInvestmentMapId == accountInvestmentMapId);

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

            _investmentMaps.RemoveAll(f => f.AccountInvestmentMapId == map.AccountInvestmentMapId);
            _investmentMaps.Add(map);
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

            _investmentMaps.Add(map);

            return new RepositoryActionResult<AccountInvestmentMap>(map, RepositoryActionStatus.Created);
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByInvestmentId(int investmentId)
        {
            return _investmentMaps.Where(inv => inv.InvestmentId == investmentId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByAccountId(int accountId)
        {
            return _investmentMaps.Where(inv => inv.AccountId == accountId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMaps()
        {
            return _investmentMaps.Select(map => new AccountInvestmentMap()
            {
                AccountId = map.AccountId,
                Valuation = map.Valuation
            }).AsQueryable();
        }

        public FundTransaction GetFundTransaction(int fundTransactionId)
        {            
            return _dummyFundTransactions.Single(t => t.FundTransactionId == fundTransactionId);
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
            
            _dummyFundTransactions.Add(dummyFundTransaction);

            return new RepositoryActionResult<FundTransaction>(dummyFundTransaction, RepositoryActionStatus.Ok);
        }

        public CashTransaction GetCashTransaction(int cashTransactionId)
        {
            return _dummyCashTransactions.Single(t => t.CashTransactionId == cashTransactionId);
        }

        public IQueryable<PriceHistory> GetInvestmentSellPrices(int investmentId)
        {
            return _dummyPriceHistoryList.Where(ph => ph.InvestmentId == investmentId).AsQueryable();
        }

        public IQueryable<PriceHistory> GetInvestmentBuyPrices(int investmentId)
        {
            return _dummyPriceHistoryList.Where(ph => ph.InvestmentId == investmentId).AsQueryable();
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

            _dummyPriceHistoryList.Add(priceHistory);

            return null;
        }

        public void SetInvestmentClass(int fakeInvestmentId, string investmentClass)
        {
            var investment = _fakeData
                .FakePopulatedInvestments()
                .First(i => i.InvestmentId == fakeInvestmentId);

            _fakeData.FakePopulatedInvestments().Remove(investment);
            investment.Class = investmentClass;
            _fakeData.FakePopulatedInvestments().Add(investment);
        }

        public void SetInvestmentIncome(int fakeInvestmentId,  string investmentIncomeType)
        {
            var investment = _fakeData
                .FakePopulatedInvestments()
                .First(i => i.InvestmentId == fakeInvestmentId);

            _fakeData.FakePopulatedInvestments().Remove(investment);
            investment.IncomeType = investmentIncomeType;
            _fakeData.FakePopulatedInvestments().Add(investment);
        }

        public List<AccountInvestmentMap> GetAllAccountInvestmentMaps()
        {
            return _investmentMaps;
        }

        public Portfolio.BackEnd.Repository.Entities.Portfolio GetPortfolio(int id)
        {
            throw new NotImplementedException();
        }

        public Portfolio.BackEnd.Repository.Entities.Portfolio GetPortfolioWithAccounts(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Portfolio.BackEnd.Repository.Entities.Portfolio> GetPortfolios()
        {
            throw new NotImplementedException();
        }
       
    }
}