using System;
using Interfaces;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordPriceHistoryProcess: IProcess
{
        private readonly PriceHistoryRequest _priceHistoryRequest;
        private readonly IPriceHistoryHandler _priceHistoryHandler;

        public RecordPriceHistoryProcess(PriceHistoryRequest priceHistoryRequest, IPriceHistoryHandler priceHistoryHandler)
        {
            _priceHistoryRequest = priceHistoryRequest;
            _priceHistoryHandler = priceHistoryHandler;        
        }

        public void Execute()
        {
            var recordedDate = DateTime.Now;
            _priceHistoryHandler.StorePriceHistory(_priceHistoryRequest, recordedDate);

            ExecuteResult = true;
        }

        public bool ProcessValid => _priceHistoryRequest.InvestmentId != 0;
        public bool ExecuteResult { get; private set; }
}

}
