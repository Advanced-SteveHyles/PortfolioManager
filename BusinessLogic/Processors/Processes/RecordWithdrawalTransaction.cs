using System.Runtime.Remoting.Contexts;
using Interfaces;
using PortfolioManager.DTO.Requests.Transactions;

namespace BusinessLogic.Transactions
{
    public class RecordWithdrawalTransaction : ICommandRunner
    {
        private readonly WithdrawalTransactionRequest _withdrawalTransactionRequest;
        private readonly IAccountHandlers _accountHandlers;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordWithdrawalTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest,
            IAccountHandlers accountHandlers, ICashTransactionHandler transactionHandler)
        {
            this._withdrawalTransactionRequest = withdrawalTransactionRequest;
            _accountHandlers = accountHandlers;
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