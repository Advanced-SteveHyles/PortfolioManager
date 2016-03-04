using Interfaces;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordWithdrawalTransaction : ICommandRunner
    {
        private readonly WithdrawalTransactionRequest _withdrawalTransactionRequest;
        private readonly IAccountHandler _accountHandler;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordWithdrawalTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest,
            IAccountHandler accountHandler, ICashTransactionHandler transactionHandler)
        {
            this._withdrawalTransactionRequest = withdrawalTransactionRequest;
            _accountHandler = accountHandler;
            _transactionHandler = transactionHandler;
        }

        public void Execute()
        {
            _transactionHandler.StoreCashTransaction(_withdrawalTransactionRequest);
            
            ExecuteResult = true;
        }

        public bool ExecuteResult { get; set; }

        public bool CommandValid => _withdrawalTransactionRequest.AccountId > 0
                                    && _withdrawalTransactionRequest.Value > 0
                                    && _withdrawalTransactionRequest.TransactionDate != null
                                    && !string.IsNullOrWhiteSpace(_withdrawalTransactionRequest.Source);
    }
}