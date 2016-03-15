using System;
using System.Web.Http;
using Portfolio.API.WebApi.Temporary;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Repositories;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
    public class TransactionSummaryController : ApiController
    {        
        private readonly AccountRepository _accountRepository;
        private readonly CashTransactionRepository _cashTransactionRepository;


        public TransactionSummaryController()
        {
            var connection = ApiConstants.Portfoliomanagercontext;
            _accountRepository = new AccountRepository(connection);
            _cashTransactionRepository = new CashTransactionRepository(connection);
        }

     //   [Route(ApiPaths.AccountTransactions)]
        //public IHttpActionResult Get(int accountId, int page = 1, int  pageSize = ApiConstants.MaxPageSize)
        public IHttpActionResult Get(int id, string fields = null) //, int page = 1, int pageSize = ApiConstants.MaxPageSize)
        {
            try
            {
               var transactionEnt = _cashTransactionRepository.GetCashTransactionsForAccount(id);
                
                var result = _accountRepository.GetAccountByAccountId(id);

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