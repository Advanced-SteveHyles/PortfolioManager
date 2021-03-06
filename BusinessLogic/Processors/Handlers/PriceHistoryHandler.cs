using System;
using System.Linq;
using Interfaces;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
{
    public class PriceHistoryHandler 
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public PriceHistoryHandler(IPriceHistoryRepository priceHistoryRepository)
        {
            this._priceHistoryRepository = priceHistoryRepository;
        }

        public void StorePriceHistory(PriceHistoryRequest priceHistoryRequest, DateTime recordedDate )
        {
            _priceHistoryRepository.InsertPriceHistory
                (
                priceHistoryRequest.InvestmentId,
                priceHistoryRequest.ValuationDate,
                priceHistoryRequest.BuyPrice,
                priceHistoryRequest.SellPrice,
                recordedDate
                );
        }

        public decimal? GetInvestmentSellPrice(int investmentId, DateTime valuationDate)
        {
            var prices = _priceHistoryRepository
                .GetInvestmentSellPrices(investmentId)
                .Where(ip => ip.ValuationDate <= valuationDate && 
                             ip.InvestmentId == investmentId && 
                             ip.SellPrice !=null)
                .OrderByDescending(ip=>ip.ValuationDate)
                .ThenByDescending(ip => ip.RecordedDate)
                .ThenByDescending(ip => ip.PriceHistoryId);

            return prices.FirstOrDefault()?.SellPrice ?? null;
        }

        public decimal? GetInvestmentBuyPrice(int investmentId, DateTime valuationDate)
        {
            var prices = _priceHistoryRepository
                .GetInvestmentBuyPrices(investmentId)
                 .Where(ip => ip.ValuationDate <= valuationDate &&
                             ip.InvestmentId == investmentId &&
                             ip.BuyPrice != null)
                .OrderByDescending(ip => ip.ValuationDate)
                .ThenByDescending(ip => ip.RecordedDate)
                .ThenByDescending(ip => ip.PriceHistoryId);

            return prices.FirstOrDefault()?.BuyPrice ?? null;
        }
    }
}