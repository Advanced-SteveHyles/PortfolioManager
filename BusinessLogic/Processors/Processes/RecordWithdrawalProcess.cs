using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordWithdrawalProcess : BaseProcess<WithdrawalTransactionRequest>
    {
        private readonly WithdrawalTransactionRequest _withdrawalTransactionRequest;
        private readonly CashTransactionHandler _transactionHandler;

        public RecordWithdrawalProcess(WithdrawalTransactionRequest request, CashTransactionHandler transactionHandler)
            :base(request)
        {
            this._withdrawalTransactionRequest = request;
            _transactionHandler = transactionHandler;            
        }

        protected override void ProcessToRun()
        {
            _transactionHandler.StoreCashTransaction(_withdrawalTransactionRequest);
        }
        
        protected override bool Validate(WithdrawalTransactionRequest request) => _withdrawalTransactionRequest.Validate();        
    }
}