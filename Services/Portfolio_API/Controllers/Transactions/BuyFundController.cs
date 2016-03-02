using System;
using System.Web.Http;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Transactions;
using Interfaces;
using PortfolioManager.DTO.DTOs.Transactions;
using PortfolioManager.DTO.Transactions;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Interfaces;
using PortfolioManager.Repository.Repositories;

namespace Portfolio_API.Controllers.Transactions
{
    public class BuyFundController : ApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly ICashTransactionRepository _cashTransactionRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentMapRepository;
        private readonly IFundTransactionRepository _fundTransactionRepository;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public BuyFundController()
        {
            var context = new PortfolioManagerContext();
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(context);
            _fundTransactionRepository = new FundTransactionRepository(context);
            _priceHistoryRepository = new PriceHistoryRepository(context);
            _cashTransactionRepository = new CashTransactionRepository(context);
            _accountRepository = new AccountRepository(context);
            _investmentRepository = new InvestmentRepository(context);
        }

        //BuyTransaction
        [System.Web.Http.HttpPost]
        [Route(ApiPaths.BuyTransaction)]
        public IHttpActionResult Post([FromBody] InvestmentBuyRequest purchaseRequest)
        {
            try
            {
                if (purchaseRequest == null)
                {
                    return BadRequest();
                }

                //var entityAccount = new AccountFactory().CreateAccount(account);
                //if (entityAccount == null)
                //{
                //    return BadRequest();
                //}

                ///*
                //{
                //    "userId": "https://expensetrackeridsrv3/embedded_1",
                //    "title": "STV",
                //    "description": "STV",
                //    "expenseGroupStatusId": 1,
                //}
                //*/

                var createFundBuyTransaction = new RecordFundBuyTransaction
                    (purchaseRequest,
                        new AccountHandler(_accountRepository),
                        new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                        new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                        new FundTransactionHandler(_fundTransactionRepository),
                        new PriceHistoryHandler(_priceHistoryRepository),
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

