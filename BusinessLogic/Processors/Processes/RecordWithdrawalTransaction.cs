using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
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
            _transactionHandler.StoreCashTransaction(_withdrawalTransactionRequest, TransactionLink.FundToCash());
            
            ExecuteResult = true;
        }

        public bool ExecuteResult { get; set; }

        public bool CommandValid => _withdrawalTransactionRequest.AccountId > 0
                                    && _withdrawalTransactionRequest.Value > 0
                                    && _withdrawalTransactionRequest.TransactionDate != null
                                    && !string.IsNullOrWhiteSpace(_withdrawalTransactionRequest.Source);
    }
}