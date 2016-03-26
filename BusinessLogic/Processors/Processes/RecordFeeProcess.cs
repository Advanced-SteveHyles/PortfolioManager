using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFeeProcess    : BaseProcess<FeeTransactionRequest>
    {
        private readonly FeeTransactionRequest _feeTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordFeeProcess(FeeTransactionRequest request, ICashTransactionHandler transactionHandler)
            :base(request)
        {            
            this._feeTransactionRequest = request;
            _transactionHandler = transactionHandler;
        }

        protected override void ProcessToRun()
        {
            _transactionHandler.StoreCashTransaction(_feeTransactionRequest);
    
        }

        protected override bool Validate(FeeTransactionRequest request) => _feeTransactionRequest.Validate();
    
    }
}