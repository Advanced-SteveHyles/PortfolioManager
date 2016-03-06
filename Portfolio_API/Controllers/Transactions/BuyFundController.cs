﻿using System;
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
            var connection = ApiConstants.Portfoliomanagercontext;
            var context = new PortfolioManagerContext(connection);
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(context);
            _fundTransactionRepository = new FundTransactionRepository(context);
            _priceHistoryRepository = new PriceHistoryRepository(context);
            _cashTransactionRepository = new CashTransactionRepository(context);
            _accountRepository = new AccountRepository(connection);
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

