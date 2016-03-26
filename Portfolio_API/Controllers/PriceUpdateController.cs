using System;
using System.Web.Http;
using Interfaces;
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
            var connection = ApiConstants.Portfoliomanagercontext;
            var context = new PortfolioManagerContext(connection);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(connection);
            _priceHistoryRepository = new PriceHistoryRepository(connection
                );
            _accountRepository = new AccountRepository(connection);
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
var priceHistoryProcessor = new RecordPriceHistoryProcess(request, historyHandler);

                var revalueSinglePriceRequest = new RevalueSinglePriceRequest
                {
                    InvestmentId = request.InvestmentId,
                    ValuationDate = request.ValuationDate
                };

                var revalueSinglePriceCommand = new RevalueSinglePriceProcess(
                    revalueSinglePriceRequest,
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
