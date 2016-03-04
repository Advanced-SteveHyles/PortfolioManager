using System;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface IPriceHistoryHandler
    {
        void StorePriceHistory(PriceHistoryRequest priceHistoryRequest, DateTime recordedDate);
        decimal? GetInvestmentSellPrice(int investmentId, DateTime valuationDate);
        decimal? GetInvestmentBuyPrice(int investmentId, DateTime valuationDate);
    }
}