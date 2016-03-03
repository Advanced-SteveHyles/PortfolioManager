﻿using System;
using System.Web.Http;
using Portfolio.API.WebApi.Temporary;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Repositories;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
    public class TransactionSummaryController : ApiController
    {        
        private AccountRepository _accountRepository;
        private CashTransactionRepository _cashTransactionRepository;


        public TransactionSummaryController()
        {
            _accountRepository = new AccountRepository(new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext));
            _cashTransactionRepository = new CashTransactionRepository(new PortfolioManagerContext(ApiConstants.Portfoliomanagercontext));
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