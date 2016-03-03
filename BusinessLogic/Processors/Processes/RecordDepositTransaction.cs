using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordDepositTransaction : ICommandRunner
    {
        private readonly DepositTransactionRequest _depositTransactionRequest;
        private readonly ICashTransactionHandler _transactionHandler;

        public RecordDepositTransaction(DepositTransactionRequest depositTransactionRequest, ICashTransactionHandler transactionHandler)
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

        public bool CommandValid
        {
            get 
            { 
            return _depositTransactionRequest.AccountId > 0
                   && _depositTransactionRequest.Value > 0
                   && _depositTransactionRequest.TransactionDate != null
                   && !string.IsNullOrWhiteSpace(_depositTransactionRequest.Source)                 
                   ;
            }

        }
    }
}
