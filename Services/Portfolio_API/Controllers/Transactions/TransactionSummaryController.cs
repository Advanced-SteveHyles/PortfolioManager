using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Interfaces;
using PortfolioManager.DTO.DTOs.Transactions;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Interfaces;
using PortfolioManager.Repository.Repositories;

namespace Portfolio_API.Controllers.Transactions
{
    public class TransactionSummaryController : ApiController
    {        
        private AccountRepository _accountRepository;
        private CashTransactionRepository _cashTransactionRepository;


        public TransactionSummaryController()
        {
            _accountRepository = new AccountRepository(new PortfolioManagerContext());
            _cashTransactionRepository = new CashTransactionRepository(new PortfolioManagerContext());
        }

     //   [Route(ApiPaths.AccountTransactions)]
        //public IHttpActionResult Get(int accountId, int page = 1, int  pageSize = ApiConstants.MaxPageSize)
        public IHttpActionResult Get(int id, string fields = null) //, int page = 1, int pageSize = ApiConstants.MaxPageSize)
        {
            try
            {
               var transactionEnt = _cashTransactionRepository.GetCashTransactionsForAccount(id);
                
                var result = _accountRepository.GetAccount(id);

                if (result != null)
                {
                    return Ok(ShapedData.CreateDataShapedObject(id, transactionEnt));
                }
                else
                {
                    return NotFound();
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