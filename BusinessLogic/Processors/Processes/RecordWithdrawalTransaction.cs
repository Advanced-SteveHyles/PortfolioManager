using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordWithdrawalTransaction : ICommandRunner
    {
        private readonly WithdrawalTransactionRequest _withdrawalTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordWithdrawalTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest, ICashTransactionHandler transactionHandler)
        {
            this._withdrawalTransactionRequest = withdrawalTransactionRequest;
            _transactionHandler = transactionHandler;            
        }

        public void Execute()
        {
            _transactionHandler.StoreCashTransaction(_withdrawalTransactionRequest);
            
            ExecuteResult = true;
        }

        public bool ExecuteResult { get; set; }

        public bool CommandValid => _withdrawalTransactionRequest.Validate();
    }
}