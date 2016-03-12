using System;
using System.Web.Http;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs.Transactions;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
    public class CorporateActionController : ApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly ICashTransactionRepository _cashTransactionRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentMapRepository;
        private readonly IFundTransactionRepository _fundTransactionRepository;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public CorporateActionController()
        {
            var connection = ApiConstants.Portfoliomanagercontext;
            var context = new PortfolioManagerContext(connection);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(connection);
            _fundTransactionRepository = new FundTransactionRepository(connection);
            _priceHistoryRepository = new PriceHistoryRepository(connection);
            _cashTransactionRepository = new CashTransactionRepository(connection);
            _accountRepository = new AccountRepository(connection);
            _investmentRepository = new InvestmentRepository(connection);
        }

        [System.Web.Http.HttpPost]
        [Route(ApiPaths.CorporateAction)]
        public IHttpActionResult Post([FromBody] InvestmentCorporateActionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest();
                }


                var createFundBuyTransaction = new RecordCorporateActionTransaction
                    (request,
                    new FundTransactionHandler(_fundTransactionRepository),
                    new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                        new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                        new InvestmentHandler(_investmentRepository)                        
                    );

                var status = Command.ExecuteCommand
                    (
                        createFundBuyTransaction
                    );

                if (status)
                {
                    //var dtoTransaction = EntityToDtoMap.MapTransactionToDto(result.Entity);
                    return Created(Request.RequestUri + "/", new CashTransactionDto());
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