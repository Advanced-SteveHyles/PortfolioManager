using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFeeProcess    : IProcess
    {
        private readonly FeeTransactionRequest _feeTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordFeeProcess(FeeTransactionRequest feeTransactionRequest, ICashTransactionHandler transactionHandler)
        {
            this._feeTransactionRequest = feeTransactionRequest;
            _transactionHandler = transactionHandler;
        }

        public void Execute()
        {
            _transactionHandler.StoreCashTransaction(_feeTransactionRequest);

            ExecuteResult = true;
        }

        public bool ExecuteResult { get; set; }

        public bool ProcessValid => _feeTransactionRequest.Validate();
    }
}