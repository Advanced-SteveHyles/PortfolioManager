using System;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountSummaryController
    {
        private readonly CashTransactionRepository _cashTransactionRepository;
        
        public VirtualAccountSummaryController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _cashTransactionRepository = new CashTransactionRepository(connection);
        }

        public IVirtualActionResult GetCashTransactionSummary(int accountId)
        {
            try
            {
                var transactions = _cashTransactionRepository.GetCashTransactionsForAccount(accountId);
                var transactionSummaries = transactions
                    .ToList()
                    .Select(transaction => transaction.MapToDto())
                    .ToList();

                return new OkMultipleActionResult<CashTransactionDto>(transactionSummaries);

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

    }
}