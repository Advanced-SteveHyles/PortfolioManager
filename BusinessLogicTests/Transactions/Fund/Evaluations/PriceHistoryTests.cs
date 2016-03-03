using System;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Processors.Processes;
using BusinessLogic.Transactions;
using BusinessLogicTests.FakeRepositories;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.Repository.Entities;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund.Evaluations
{
    public class PriceHistoryTests
    {
        private readonly FakeRepository _repository;
        private readonly PriceHistoryHandler _priceHistoryHandler;
        private RecordPriceHistoryProcessor _recordPriceHistoryProcessor;

        private int investmentId = 629;

        private DateTime todaysValuationDate = DateTime.Today;
        decimal? todaysBuyPrice = (decimal)1.25;
        decimal? todaysSellPrice = (decimal)1.25;
        private RevalueSinglePriceCommand _revalueSinglePriceCommand;


        public PriceHistoryTests()
        {
            _repository = new FakeRepository();
            _priceHistoryHandler = new PriceHistoryHandler(_repository);

            var accountMapHandler = new AccountHandler(_repository);
            var investmentMapProcessor = new AccountInvestmentMapProcessor(_repository);
            _revalueSinglePriceCommand = new RevalueSinglePriceCommand(investmentId, todaysValuationDate, _priceHistoryHandler, investmentMapProcessor, accountMapHandler);
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

            _recordPriceHistoryProcessor = new RecordPriceHistoryProcessor(
                priceHistoryRequest, _priceHistoryHandler);
        }

        [Fact]
        public void CanSaveAPriceHistory()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);

            Assert.True(_recordPriceHistoryProcessor.CommandValid);
            _recordPriceHistoryProcessor.Execute();
            Assert.True(_recordPriceHistoryProcessor.ExecuteResult);

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
            _recordPriceHistoryProcessor.Execute();

            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            Assert.Equal(todaysSellPrice, currentSellPrice);
            Assert.Equal(todaysBuyPrice, currentBuyPrice);
        }

        [Fact]
        public void WhenAFutureDatePriceExistsTheCurrentPriceIsTheClosestBeforeTheCurrentDate()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcessor.Execute();

            var tomorrowsDate = DateTime.Today.AddDays(1);
            decimal? tomorrowsBuyPrice = (decimal)2.25;
            decimal? tomorrowsSellPrice = (decimal)2.25;
            SetupPriceHistory(tomorrowsDate, tomorrowsBuyPrice, tomorrowsSellPrice);
            _recordPriceHistoryProcessor.Execute();


            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, todaysValuationDate);
            var currentBuyPrice = _priceHistoryHandler.GetInvestmentBuyPrice(investmentId, todaysValuationDate);

            Assert.Equal(todaysSellPrice, currentSellPrice);
            Assert.Equal(todaysBuyPrice, currentBuyPrice);
        }


        [Fact]
        public void WhenTwoPricesExistForTheCurrentDateTheOneAddedLastIsUsed()
        {
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice, todaysSellPrice);
            _recordPriceHistoryProcessor.Execute();

            decimal? latestBuyPrice = (decimal)3.25;
            decimal? latestSellPrice = (decimal)5.25;

            SetupPriceHistory(todaysValuationDate, latestBuyPrice, latestSellPrice);
            _recordPriceHistoryProcessor.Execute();
            

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
            _recordPriceHistoryProcessor.Execute();
            _revalueSinglePriceCommand.Execute();

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
            _recordPriceHistoryProcessor.Execute();
            _revalueSinglePriceCommand.Execute();

            var valuation1 = todaysBuyPrice * request1.Quantity;
            var valuation2 = todaysBuyPrice * request2.Quantity;

            var investmentMap1 = _repository.GetAccount(1);
            var investmentMap2 = _repository.GetAccount(2);

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
            _recordPriceHistoryProcessor.Execute();
            _revalueSinglePriceCommand.Execute();

            var valuation1 = todaysSellPrice * request1.Quantity;
            var valuation2 = todaysSellPrice * request2.Quantity;

            var account1 = _repository.GetAccount(1);
            var account2 = _repository.GetAccount(2);


            Assert.Equal(valuation1, account1.Valuation);
            Assert.Equal(valuation2, account2.Valuation);

            var sellPriceIncrement = (decimal)1.5;
            valuation1 = (todaysSellPrice + sellPriceIncrement) * request1.Quantity;            
            valuation2 = (todaysSellPrice + sellPriceIncrement) * request2.Quantity;
            SetupPriceHistory(todaysValuationDate, todaysBuyPrice+1, todaysSellPrice+ sellPriceIncrement);
            _recordPriceHistoryProcessor.Execute();
            _revalueSinglePriceCommand.Execute();

            account1 = _repository.GetAccount(1);
             account2 = _repository.GetAccount(2);


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
