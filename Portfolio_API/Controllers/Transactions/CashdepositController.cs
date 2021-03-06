﻿using System;
using System.Web.Http;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs.Transactions;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.WebApi.Controllers.Transactions
{

    public class CashdepositController : ApiController
    {
        readonly ICashTransactionRepository _cashTransactionRepository;
        readonly IAccountRepository _accountRepository;


        public CashdepositController()
        {
            var connection = ApiConstants.Portfoliomanagercontext;
            _cashTransactionRepository = new CashTransactionRepository(connection);
            _accountRepository = new AccountRepository(connection);
        }

        [System.Web.Http.HttpPost]
        [Route(ApiPaths.CashDeposit)]
        public IHttpActionResult Post([FromBody] DepositTransactionRequest deposit)
        {
            try
            {
                if (deposit == null)
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
                var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

                var recordDepositProcess = new RecordDepositProcess(deposit,
                        transactionHandler,
                        TransactionLink.FundToCash());
                
                recordDepositProcess.Execute();

                if (recordDepositProcess.ExecuteResult)                
                {                    
                    //var dtoTransaction = EntityToDtoMap.MapTransactionToDto(result.Entity);
                    return Created(Request.RequestUri + "/" + deposit.AccountId, new CashTransactionDto());
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
