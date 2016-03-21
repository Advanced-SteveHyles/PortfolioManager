using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCashTransferTransaction : ICommandRunner
    {
        private readonly CashTransferRequest _request;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        

        public RecordCashTransferTransaction(CashTransferRequest request, ICashTransactionHandler cashTransactionHandler)
        {
            _request = request;
            _cashTransactionHandler = cashTransactionHandler;       
        }

        public void Execute()
        {
            ExecuteResult = true;
        }

        public bool CommandValid => _request.Validate();
        public bool ExecuteResult { get; private set; }
    }
}