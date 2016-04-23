using System;
using System.Linq;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund.Evaluations
{
    public class FullPortfolioRevaluation
    {
        private readonly FakeRepository _fakeRepository = new FakeRepository(new FakeDataGeneric());
        private readonly PriceHistoryHandler _priceHandler;

        public FullPortfolioRevaluation()
        {
            _priceHandler = new PriceHistoryHandler(_fakeRepository);
        }

        [Fact]
        public void WhenIPerformAMassValuationAllMapsAreCorrect()
        {
            RunForYesterdaysPrice();
            foreach (var map in _fakeRepository.GetAllAccountInvestmentMaps().ToList())
            {
                var price = _fakeRepository.GetInvestmentSellPrices(map.InvestmentId).FirstOrDefault();
                var valuation = (price?.SellPrice) * map.Quantity;

                Assert.Equal(valuation, map.Valuation);
            }
        }

        [Fact]
        public void WhenTheAccountHasNoMapsAccountsIsCorrect()
        {
            RunForYesterdaysPrice();
            Assert.Equal(0, _fakeRepository.GetAccountByAccountId(5).Valuation);
        }

        [Fact]
        public void WhenIPerformAMassValuationWithOneHistoryAllAccountsAreCorrect()
        {
            //new AccountInvestmentMapDto()
            //{
            //    InvestmentId = 1, AccountId = 1, Quantity = 10,           //10
            //    InvestmentId = 2, AccountId = 1, Quantity = 10,           //7
            //    InvestmentId = 1, AccountId = 2, Quantity = 5,            //10
            //    InvestmentId = 1, AccountId = 3, Quantity = 25.4          //10
            //    InvestmentId = 1, AccountId = 4, Quantity = 1.78923       //10
            //    InvestmentId = 3, AccountId = 6, Quantity = 21            //12.5
            RunForYesterdaysPrice();
            Assert.Equal(170, _fakeRepository.GetAccountByAccountId(1).Valuation);
            Assert.Equal(50, _fakeRepository.GetAccountByAccountId(2).Valuation);
            Assert.Equal(254, _fakeRepository.GetAccountByAccountId(3).Valuation);
            Assert.Equal((decimal)17.89230, _fakeRepository.GetAccountByAccountId(4).Valuation);
            Assert.Equal((decimal)262.5, _fakeRepository.GetAccountByAccountId(6).Valuation);
        }

        [Fact]
        public void WhenIPerformAMassValuationWithTwoHistoriesAllAccountsAreCorrect()
        {
            //new AccountInvestmentMapDto()
            //{
            //    InvestmentId = 1, AccountId = 1, Quantity = 10,           //10.6
            //    InvestmentId = 2, AccountId = 1, Quantity = 10,           //7.1
            //    InvestmentId = 1, AccountId = 2, Quantity = 5,            //10.6
            //    InvestmentId = 1, AccountId = 3, Quantity = 25.4          //10.6
            //    InvestmentId = 1, AccountId = 4, Quantity = 1.78923       //10.6
            //    InvestmentId = 3, AccountId = 6, Quantity = 21            //0
            RunForYesterdaysPrice();
            RunForTodaysPrice();
            Assert.Equal(177, _fakeRepository.GetAccountByAccountId(1).Valuation);
            Assert.Equal(53, _fakeRepository.GetAccountByAccountId(2).Valuation);
            Assert.Equal((decimal)269.24, _fakeRepository.GetAccountByAccountId(3).Valuation);
            Assert.Equal((decimal)18.965838, _fakeRepository.GetAccountByAccountId(4).Valuation);
            Assert.Equal(0, _fakeRepository.GetAccountByAccountId(6).Valuation);
        }

        private void RunForYesterdaysPrice()
        {
            var valuationDate = DateTime.Today.AddDays(-1);
            SetupPrice(1, valuationDate, 10);
            SetupPrice(2, valuationDate, 7);
            SetupPrice(3, valuationDate, (decimal)12.5);
            SetupPrice(4, valuationDate, (decimal)2.442);

            SetupMassUpdate();
        }

        private void RunForTodaysPrice()
        {            
            SetupPrice(1, DateTime.Today, (decimal)10.6);
            SetupPrice(2, DateTime.Today, (decimal)7.1);
            SetupPrice(3, DateTime.Today, 0);
            SetupPrice(4, DateTime.Today, 4);

            SetupMassUpdate();
        }

        private void SetupPrice(int investmentId, DateTime valuationDate, decimal sellPrice)
        {
            var priceHistoryRequest = new PriceHistoryRequest()
            {
                InvestmentId = investmentId,
                ValuationDate = valuationDate,
                SellPrice = sellPrice
            };
            _priceHandler.StorePriceHistory(priceHistoryRequest, DateTime.Now);         
        }

        private void SetupMassUpdate()
        {
            var request = new RevalueAllPricesRequest() { EvaluationDate = DateTime.Now };
            var revalueAllPricesCommand = new RevalueAllPricesProcess(
                request, new AccountInvestmentMapProcessor(_fakeRepository),
                new InvestmentHandler(_fakeRepository),
                new PriceHistoryHandler(_fakeRepository),
                new AccountHandler(_fakeRepository)
                );

            revalueAllPricesCommand.Execute();
        }
    }
}
