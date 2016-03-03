using System;
using Interfaces;
using PortfolioManager.DTO.Requests.Transactions;

namespace BusinessLogic.Processors.Processes
{
    public class RecordPriceHistoryProcessor: ICommandRunner
{
        private readonly PriceHistoryRequest _priceHistoryRequest;
        private readonly IPriceHistoryHandler _priceHistoryHandler;

        public RecordPriceHistoryProcessor(PriceHistoryRequest priceHistoryRequest, IPriceHistoryHandler priceHistoryHandler)
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

        public bool CommandValid => _priceHistoryRequest.InvestmentId != 0;
        public bool ExecuteResult { get; private set; }
}

}
