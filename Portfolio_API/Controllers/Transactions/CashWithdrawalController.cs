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
public class CashWithdrawalController : ApiController
    {
        readonly ICashTransactionRepository _cashTransactionRepository;
        readonly IAccountRepository _accountRepository;
        public CashWithdrawalController()
        {
            var connection = ApiConstants.Portfoliomanagercontext;
            _cashTransactionRepository = new CashTransactionRepository(connection);
            _accountRepository = new AccountRepository(connection);
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

                var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

                var withdrawalProcess = new RecordWithdrawalProcess(withdrawal, transactionHandler);
                
                withdrawalProcess.Execute();

                if (withdrawalProcess.ExecuteResult)
                {
                    //var dtoTransaction = EntityToDtoMap.MapTransactionToDto(result.Entity);
                    return Created(Request.RequestUri + "/" + withdrawal.AccountId, new CashTransactionDto());
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
