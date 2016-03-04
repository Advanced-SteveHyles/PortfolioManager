using System;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests.Transactions;

//api/singlepriceupdate
namespace Portfolio.API.Virtual.VirtualControllers
{
    public class PriceUpdateController
    {
        private readonly AccountRepository _accountRepository;
        private readonly PriceHistoryRepository _priceHistoryRepository;
        private readonly AccountInvestmentMapRepository _accountInvestmentMapRepository;

        public PriceUpdateController(string connection)
        {
            var context = new PortfolioManagerContext(connection);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(context);
            _priceHistoryRepository = new PriceHistoryRepository(context);
            _accountRepository = new AccountRepository(context);
        }

        public IVirtualActionResult Singlepriceupdate_Post(PriceHistoryRequest request)
        {

            try
            {
                if (request == null)
                {
                    return new BadRequest();
                }

                var entityPriceHistory = new PriceHistoryFactory().CreatePriceHistory(request);
                if (entityPriceHistory == null)
                {
                    return new BadRequest();
                }

                /*
                {
                    "userId": "https://expensetrackeridsrv3/embedded_1",
                    "title": "STV",
                    "description": "STV",
                    "expenseGroupStatusId": 1,
                }
                */
                var historyHandler = new PriceHistoryHandler(_priceHistoryRepository);
                var priceHistoryProcessor = new RecordPriceHistoryProcessor(request, historyHandler);
                var revalueSinglePriceCommand = new RevalueSinglePriceCommand(
                    request.InvestmentId,
                    request.ValuationDate,
                    historyHandler,
                    new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                    new AccountHandler(_accountRepository)
                    );

                priceHistoryProcessor.Execute();
                revalueSinglePriceCommand.Execute();

                if (priceHistoryProcessor.ExecuteResult && revalueSinglePriceCommand.ExecuteResult)
                {
                    //var dtoPortfolio = result.Entity.MapToDto();
                    return new Created(new { });
                }
                else
                {
                    return new BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerError();
            }
        }
    }
}
