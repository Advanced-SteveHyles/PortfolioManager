using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordDepositProcess : IProcess
    {
        private readonly DepositTransactionRequest _depositTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;
        
        public RecordDepositProcess(DepositTransactionRequest depositTransactionRequest, ICashTransactionHandler transactionHandler, TransactionLink transactionLink)
        {
            this._depositTransactionRequest = depositTransactionRequest;
            _transactionHandler = transactionHandler;            
        }

        public void Execute()
        {            
            _transactionHandler.StoreCashTransaction(_depositTransactionRequest);                                 
            ExecuteResult = true;
        }

        public bool ExecuteResult { get; set; }

        public bool ProcessValid => _depositTransactionRequest.Validate();        
    }
}
