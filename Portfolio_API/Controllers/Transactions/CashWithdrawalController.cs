﻿using System;
using System.Web.Http;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs.Transactions;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
public class CashWithdrawalController : ApiController
    {
        readonly ICashTransactionRepository _cashTransactionRepository;
        readonly IAccountRepository _accountRepository;
        public CashWithdrawalController()
        {
            _cashTransactionRepository = new CashTransactionRepository(new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext));
            _accountRepository = new AccountRepository(new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext));
        }


        [System.Web.Http.HttpPost]
        [Route(ApiPaths.CashWithdrawal)]
        public IHttpActionResult Post([FromBody] WithdrawalTransactionRequest  withdrawal)
        {
            try
            {
                if (withdrawal == null)
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

                var accountHandler = new AccountHandler(_accountRepository);
                var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

                var status = Command.ExecuteCommand(new RecordWithdrawalTransaction(withdrawal, accountHandler, transactionHandler));

                if (status)
                {
                    //var dtoTransaction = EntityToDtoMap.MapTransactionToDto(result.Entity);
                    return Created(Request.RequestUri + "/" + withdrawal.AccountId, new TransactionDto());
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