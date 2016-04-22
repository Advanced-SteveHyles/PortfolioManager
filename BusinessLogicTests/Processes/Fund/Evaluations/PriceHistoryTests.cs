using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund.Evaluations
{
    public class PriceHistoryTests
    {
        private readonly FakeRepository _repository;
        private readonly PriceHistoryHandler _priceHistoryHandler;
        private RecordPriceHistoryProcess _recordPriceHistoryProcess;

        private int investmentId = 629;

        private DateTime todaysValuationDate = DateTime.Today;
        decimal? todaysBuyPrice = (decimal)1.25;
        decimal? todaysSellPrice = (decimal)1.25;
        private RevalueSinglePriceProcess _revalueSinglePriceProcess;


        public PriceHistoryTests()
        {
            _repository = new FakeRepository(new FakeDataGeneric());
            _priceHistoryHandler = new PriceHistoryHandler(_repository);

            var accountMapHandler = new AccountHandler(_repository);
            var investmentMapProcessor = new AccountInvestmentMapProcessor(_repository);

            var request = new RevalueSinglePriceRequest()
            {
                InvestmentId =  investmentId,                 
                ValuationDate = todaysValuationDate
            };
            _revalueSinglePriceProcess = new RevalueSinglePriceProcess(request, _priceHistoryHandler, investmentMapProcessor, accountMapHandler);
        }

        private void SetupPriceHistory(DateTime valuationDate, decimal? buyAt, decimal? sellAt)
        {
            var priceHistoryRequest = new PriceHistoryRequest()
            {
                InvestmentId = investmentId,
                ValuationDate = valuationDate,
                SellPrice = sellAt,
                BuyPrice = buyAt
            };

            _recordPriceHistoryProcess = new RecordPriceHistoryProcess(
                priceHistoryRequest, _priceHistoryHandler);
        }

        [Fact]
        public void CanSaveAPriceHistory()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            
            _recordPriceHistoryProcess.Execute();
            Assert.True(_recordPriceHistoryProcess.ExecuteResult);

            var priceHistory = _repository.GetInvestmentSellPrices(investmentId);
            Assert.Equal(investmentId, priceHistory.FirstOrDefault()?.InvestmentId);
            Assert.Equal(todaysValuationDate, priceHistory.FirstOrDefault()?.ValuationDate);
            Assert.Equal(todaysSellPrice, priceHistory.FirstOrDefault()?.SellPrice);
            Assert.Equal(todaysBuyPrice, priceHistory.FirstOrDefault()?.BuyPrice);
        }

        [Fact]
        public void WhenNoHistoricalPriceExistsTheCurrentPriceIsNull()
        {
            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            decimal? notDefinedSellPrice = null;
            decimal? notDefinedBuyPrice = null;

            Assert.Equal(notDefinedSellPrice, currentSellPrice);
            Assert.Equal(notDefinedBuyPrice, currentBuyPrice);
        }


        [Fact]
        public void WhenAHistoricalPriceExistsTheCurrentPriceIsTheClosestBeforeTheCurrentDate()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();

            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            Assert.Equal(todaysSellPrice, currentSellPrice);
            Assert.Equal(todaysBuyPrice, currentBuyPrice);
        }

        [Fact]
        public void WhenAFutureDatePriceExistsTheCurrentPriceIsTheClosestBeforeTheCurrentDate()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();

            var tomorrowsDate = DateTime.Today.AddDays(1);
            decimal? tomorrowsBuyPrice = (decimal)2.25;
            decimal? tomorrowsSellPrice = (decimal)2.25;
            SetupPriceHistory(tomorrowsDate, tomorrowsBuyPrice, tomorrowsSellPrice);
            _recordPriceHistoryProcess.Execute();


            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            Assert.Equal(todaysSellPrice, currentSellPrice);
            Assert.Equal(todaysBuyPrice, currentBuyPrice);
        }


        [Fact]
        public void WhenTwoPricesExistForTheCurrentDateTheOneAddedLastIsUsed()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();

            decimal? latestBuyPrice = (decimal)3.25;
            decimal? latestSellPrice = (decimal)5.25;

            SetupPriceHistory(todaysValuationDate, latestBuyPrice, latestSellPrice);
            _recordPriceHistoryProcess.Execute();
            

            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            Assert.Equal(latestBuyPrice, currentBuyPrice);
            Assert.Equal(latestSellPrice, currentSellPrice);
        }



        [Fact]
        public void WhenIHaveTwoInvestmentMapsForTheSameInvestmentAndIUpdateThePriceBothInvestmentsUpdate()
        {
            var newInvestmentMap1 = 5;
            var request1 = CreateDummyInvestmentMap(newInvestmentMap1, 1, investmentId, 1);
            _repository.InsertAccountInvestmentMap(request1);

            var newInvestment2 = 6;
            var request2 = CreateDummyInvestmentMap(newInvestment2, 2, investmentId, 100);
            _repository.InsertAccountInvestmentMap(request2);

            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();
            _revalueSinglePriceProcess.Execute();

            var valuation1 = todaysBuyPrice * request1.Quantity;
            var valuation2 = todaysBuyPrice * request2.Quantity;

            var investmentMap1 = _repository.GetAccountInvestmentMap(newInvestmentMap1);
            var investmentMap2 = _repository.GetAccountInvestmentMap(newInvestment2);

            Assert.Equal(valuation1, investmentMap1.Valuation);
            Assert.Equal(valuation2, investmentMap2.Valuation);
        }

        [Fact]
        public void WhenIHaveTwoInvestmentMapsForTheSameInvestmentAndIUpdateTheAccountValuationUpdate()
        {
            var newInvestmentMap1 = 5;
            var request1 = CreateDummyInvestmentMap(newInvestmentMap1, 1, investmentId, 1);
            _repository.InsertAccountInvestmentMap(request1);

            var newInvestmentMap2 = 6;
            var request2 = CreateDummyInvestmentMap(newInvestmentMap2, 2, investmentId, 100);
            _repository.InsertAccountInvestmentMap(request2);

            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();
            _revalueSinglePriceProcess.Execute();

            var valuation1 = todaysBuyPrice * request1.Quantity;
            var valuation2 = todaysBuyPrice * request2.Quantity;

            var investmentMap1 = _repository.GetAccountByAccountId(1);
            var investmentMap2 = _repository.GetAccountByAccountId(2);

            Assert.Equal(valuation1, investmentMap1.Valuation);
            Assert.Equal(valuation2, investmentMap2.Valuation);
        }

        [Fact]
        public void WhenTwoPriceUpdatesOccurAndIHaveTwoInvestmentMapsForTheSameInvestmentAndIUpdateTheAccountValuationUpdatesCorrectly()
        {
            var newInvestmentMap1 = 5;
            var request1 = CreateDummyInvestmentMap(newInvestmentMap1, 1,investmentId, 20);
            _repository.InsertAccountInvestmentMap(request1);

            var newInvestmentMap2 = 6;
            var request2 = CreateDummyInvestmentMap(newInvestmentMap2, 2, investmentId, (decimal)25.045);
            _repository.InsertAccountInvestmentMap(request2);

            SetupPriceHistory(todaysValuationDate.AddDays(-1), todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcess.Execute();
            _revalueSinglePriceProcess.Execute();

            var valuation1 = todaysSellPrice * request1.Quantity;
            var valuation2 = todaysSellPrice * request2.Quantity;

            var account1 = _repository.GetAccountByAccountId(1);
            var account2 = _repository.GetAccountByAccountId(2);


            Assert.Equal(valuation1, account1.Valuation);
            Assert.Equal(valuation2, account2.Valuation);

            var sellPriceIncrement = (decimal)1.5;
            valuation1 = (todaysSellPrice + sellPriceIncrement) * request1.Quantity;            
            valuation2 = (todaysSellPrice + sellPriceIncrement) * request2.Quantity;
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice+1, todaysSellPrice+ sellPriceIncrement);
            _recordPriceHistoryProcess.Execute();
            _revalueSinglePriceProcess.Execute();

            account1 = _repository.GetAccountByAccountId(1);
             account2 = _repository.GetAccountByAccountId(2);


            Assert.Equal(valuation1, account1.Valuation);
            Assert.Equal(valuation2, account2.Valuation);
        }
        

        private AccountInvestmentMap CreateDummyInvestmentMap(int accountInvestmentMapId, int accountId, int investmentId, decimal quantity)
        {
            return new AccountInvestmentMap()
            {
                AccountId = accountId,
                AccountInvestmentMapId = accountInvestmentMapId,
                InvestmentId = investmentId,
                Quantity = quantity
            };
        }
        
    }
}
