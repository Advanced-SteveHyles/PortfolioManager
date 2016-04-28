using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.Fakes
{
    public class FakePriceHistoryRepository : IPriceHistoryRepository
    {

        private readonly FakeData _fakeData;

        public FakePriceHistoryRepository(FakeData fakeData)
        {
            _fakeData = fakeData;
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

        public void SetInvestmentIncome(int fakeInvestmentId, string investmentIncomeType)
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