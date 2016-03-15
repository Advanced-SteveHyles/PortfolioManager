using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<CashTransactionDto> GetCashTransactionSummary(int accountId)
        {

            var transactions = _cashTransactionRepository.GetCashTransactionsForAccount(accountId);
            var transactionSummaries = transactions
                .ToList()
                .Select(transaction => transaction.MapToDto())
                .ToList();

            return transactionSummaries;
        }

    }
}