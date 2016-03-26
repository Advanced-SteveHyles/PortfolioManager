using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordDepositProcess : BaseProcess<DepositTransactionRequest>
    {
        private readonly DepositTransactionRequest _depositTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;
        
        public RecordDepositProcess(DepositTransactionRequest request, ICashTransactionHandler transactionHandler, TransactionLink transactionLink)
            :base(request)
        {
            
            this._depositTransactionRequest = request;
            _transactionHandler = transactionHandler;            
        }

        protected override void ProcessToRun()
        {            
            _transactionHandler.StoreCashTransaction(_depositTransactionRequest);                                 

        }

        protected override bool Validate(DepositTransactionRequest request) => _depositTransactionRequest.Validate();        

    }
}
