using System;
using System.Web.Http;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Transactions;
using Interfaces;
using PortfolioManager.DTO.DTOs.Transactions;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Interfaces;
using PortfolioManager.Repository.Repositories;

namespace Portfolio_API.Controllers.Transactions
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
            var context = new PortfolioManagerContext();
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