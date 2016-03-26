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

            var createDepositTransaction = new RecordDepositProcess(request, transactionHandler,null);

            createDepositTransaction.Execute();
            
            if (createDepositTransaction.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }

        public void InsertWithdrawal(WithdrawalTransactionRequest request)
        {
            var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

            var createWithdrawalTransaction = new RecordWithdrawalProcess(request, transactionHandler);

            createWithdrawalTransaction.Execute();
            
            if (createWithdrawalTransaction.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }

        public void InsertFee(FeeTransactionRequest request)
        {
            var transactionHandler = new CashTransactionHandler(_cashTransactionRepository, _accountRepository);

            var createFeeProcess = new RecordFeeProcess(request, transactionHandler);

            createFeeProcess.Execute();

            if (createFeeProcess.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }
      

    }
}