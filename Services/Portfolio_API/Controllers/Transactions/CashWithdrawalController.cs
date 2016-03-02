using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Antlr.Runtime.Tree;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Transactions;
using Interfaces;
using PortfolioManager.DTO;
using PortfolioManager.DTO.DTOs.Transactions;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Interfaces;
using PortfolioManager.Repository.Repositories;

namespace Portfolio_API.Controllers.Transactions
{
public class CashWithdrawalController : ApiController
    {
        readonly ICashTransactionRepository _cashTransactionRepository;
        readonly IAccountRepository _accountRepository;
        public CashWithdrawalController()
        {
            _cashTransactionRepository = new CashTransactionRepository(new PortfolioManagerContext());
            _accountRepository = new AccountRepository(new PortfolioManagerContext());
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
