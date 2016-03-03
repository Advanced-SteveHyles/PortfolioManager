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
            var context = new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(context);
            _fundTransactionRepository = new FundTransactionRepository(context);
            _priceHistoryRepository = new PriceHistoryRepository(context);
            _cashTransactionRepository = new CashTransactionRepository(context);
            _accountRepository = new AccountRepository(context);
            _investmentRepository = new InvestmentRepository(context);
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
                    return Created(Request.RequestUri + "/", new TransactionDto());
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