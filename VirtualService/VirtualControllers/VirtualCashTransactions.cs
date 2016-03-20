using System;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualCashTransactions
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICashTransactionRepository _cashTransactionRepository;

        public VirtualCashTransactions()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _cashTransactionRepository = new CashTransactionRepository(connection);
            _accountRepository = new AccountRepository(connection);
        }

        public void InsertDeposit(DepositTransactionRequest request)
        {            
            var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

            var createDepositTransaction = new RecordDepositTransaction(request, transactionHandler,null);

            var status = CommandExecutor.ExecuteCommand
                (
                    createDepositTransaction
                );

            if (status == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }
    }
}