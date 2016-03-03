using System;
using System.Web.Http;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.WebApi.Controllers
{
    public class PriceUpdateController : ApiController
    {        
        private readonly AccountRepository _accountRepository;
        private readonly PriceHistoryRepository _priceHistoryRepository;
        private AccountInvestmentMapRepository _accountInvestmentMapRepository;

        public PriceUpdateController()
        {
            var context = new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(context);
            _priceHistoryRepository = new PriceHistoryRepository(context);
            _accountRepository = new AccountRepository(context);
        }
        
        [System.Web.Http.HttpPost]
        [Route(ApiPaths.InvestmentSinglePriceUpdate)]
        public IHttpActionResult Post([FromBody] PriceHistoryRequest request)
        {

            try
            {
                if (request == null)
                {
                    return BadRequest();
                }

                var entityPriceHistory = new PriceHistoryFactory().CreatePriceHistory(request);
                if (entityPriceHistory == null)
                {
                    return BadRequest();
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
                    return Created(Request.RequestUri +"/", new { });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return InternalServerError();
            }
        }
    }
}
