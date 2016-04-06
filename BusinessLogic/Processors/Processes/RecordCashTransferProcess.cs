using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCashTransferProcess : BaseProcess<CashTransferRequest>
    {
        private readonly CashTransferRequest _request;
        private readonly CashTransactionHandler _cashTransactionHandler;
        private readonly AccountHandler _accountHandler;
        
        public RecordCashTransferProcess(CashTransferRequest request, CashTransactionHandler cashTransactionHandler, AccountHandler accountHandler)
            : base(request)
        {            
            _request = request;
            _cashTransactionHandler = cashTransactionHandler;
            _accountHandler = accountHandler;
        }

        protected override void ProcessToRun()
        {
            var accountFrom = _accountHandler.GetAccount(_request.FromAccount)?.Name;
            var accountTo = _accountHandler.GetAccount(_request.ToAccount)?.Name;
            var source = $"TFR {accountFrom} => {accountTo}";

            var linkedTransaction = TransactionLink.CashToCash();
            _cashTransactionHandler.StoreCashTransaction(_request, linkedTransaction, source);
        }
        
        protected override bool Validate(CashTransferRequest request) => request.Validate();        
    }
}