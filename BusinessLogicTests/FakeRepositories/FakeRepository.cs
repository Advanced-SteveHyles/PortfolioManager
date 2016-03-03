using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogicTests.FakeRepositories
{
    public class FakeRepository        :
        IInvestmentRepository
        , IAccountRepository
        , ICashTransactionRepository
        , IAccountInvestmentMapRepository
        , IFundTransactionRepository
        , IPriceHistoryRepository
    {
        private readonly Investment _investment = new Investment();        
        private readonly List<CashTransaction> _dummyCashTransactions;
        private readonly List<FundTransaction> _dummyFundTransactions;

        private readonly List<PriceHistory> _dummyPriceHistoryList;
        private readonly List<AccountInvestmentMap> _investmentMaps;

        readonly List<Account> _accounts;

        public FakeRepository()
        {
            _dummyCashTransactions = new List<CashTransaction>();
            _dummyFundTransactions = new List<FundTransaction>();
            _dummyPriceHistoryList = new List<PriceHistory>();
            _investmentMaps = FakeData.FakePopulatedInvestmentMap();

            _accounts = FakeData.FakeAccountData();
        }
        
        public RepositoryActionResult<Account> InsertAccount(Account entityAccount)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountWithInvestments(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccount(int id)
        {
            return _accounts.Single(a => a.AccountId == id);
        }

        public void IncreaseAccountBalance(int accountId, decimal amount)
        {
            GetAccount(accountId).Cash += amount;
        }

        public void DecreaseAccountBalance(int accountId, decimal amount)
        {
            GetAccount(accountId).Cash -= amount;
        }

        public void IncreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccount(accountId);
            account.Valuation += valuation;
            _accounts.RemoveAll(acc => acc.AccountId == accountId);
            _accounts.Add(account);
        }

        public void DecreaseValuation(int accountId, decimal valuation)
        {
            var account = GetAccount(accountId);
            account.Valuation -= valuation;
            _accounts.RemoveAll(acc => acc.AccountId == accountId);
            _accounts.Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accounts;
        }

        public void SetValuation(int accountId, decimal valuation)
        {
            var account = GetAccount(accountId);
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
            return _investment;
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
                TransactionType = request.TransactionType
            };
            _dummyCashTransactions.Add(
                cashTransaction
                );
            
            return null;
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
                TransactionValue = request.TransactionValue
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
            _investment.Class = investmentClass;
        }

        public List<AccountInvestmentMap> GetAllAccountInvestmentMaps()
        {
            return _investmentMaps;
        }
    }
}