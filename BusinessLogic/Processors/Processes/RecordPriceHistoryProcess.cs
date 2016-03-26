using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordPriceHistoryProcess : BaseProcess<PriceHistoryRequest>
    {
        private readonly PriceHistoryRequest _priceHistoryRequest;
        private readonly IPriceHistoryHandler _priceHistoryHandler;

        public RecordPriceHistoryProcess(PriceHistoryRequest priceHistoryRequest, IPriceHistoryHandler priceHistoryHandler)
            :base(priceHistoryRequest)
        {
            _priceHistoryRequest = priceHistoryRequest;
            _priceHistoryHandler = priceHistoryHandler;
        }

        protected override void ProcessToRun()
        {
            var recordedDate = DateTime.Now;
            _priceHistoryHandler.StorePriceHistory(_priceHistoryRequest, recordedDate);
        }

        protected override bool Validate(PriceHistoryRequest request) => _priceHistoryRequest.InvestmentId != 0;

    }

}
