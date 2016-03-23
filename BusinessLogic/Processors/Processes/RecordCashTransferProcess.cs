using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCashTransferProcess : IProcess
    {
        private readonly CashTransferRequest _request;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountHandler _accountHandler;


        public RecordCashTransferProcess(CashTransferRequest request, ICashTransactionHandler cashTransactionHandler, IAccountHandler accountHandler)
        {
            _request = request;
            _cashTransactionHandler = cashTransactionHandler;
            _accountHandler = accountHandler;
        }

        public void Execute()
        {
            var accountFrom = _accountHandler.GetAccount(_request.FromAccount)?.Name;
            var accountTo = _accountHandler.GetAccount(_request.ToAccount)?.Name;
            var source = $"TFR {accountFrom} => {accountTo}";

            var linkedTransaction = TransactionLink.CashToCash();
            _cashTransactionHandler.StoreCashTransaction(_request, linkedTransaction, source);
            
            ExecuteResult = true;
        }

        public bool ProcessValid => _request.Validate();
        public bool ExecuteResult { get; private set; }
    }
}