using System;
using System.Linq;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCashCheckpointProcess : BaseProcess<CheckpointRequest>
    {
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly CheckpointRequest _checkpointRequest;
        private readonly ICashTransactionRepository _cashTransactionHandler;

        public RecordCashCheckpointProcess(ICheckpointRepository checkpointRepository, ICashTransactionRepository cashTransactionHandler, CheckpointRequest request) : base(request)
        {
            _checkpointRepository = checkpointRepository;
            _checkpointRequest = request;
            _cashTransactionHandler = cashTransactionHandler;
        }
        
        protected override void ProcessToRun()
        {
            var previousCheckpoint = _checkpointRepository.GetLastestCheckpointForAccount(_checkpointRequest.AccountId);
            var closingTotal = _checkpointRequest.ItemsToCheckpoint.Sum(tx => tx.TransactionValue);
            var openingValue = previousCheckpoint?.ClosingValue ?? 0;

            var cashCheckpoint = _checkpointRepository.InsertCheckpoint(_checkpointRequest, openingValue, closingTotal);

            if (cashCheckpoint.Status != RepositoryActionStatus.Ok)
            {
                //ExecuteResult = false;
                return;
            }        
            
            foreach (var cashTransaction in _checkpointRequest.ItemsToCheckpoint)
            {
                _cashTransactionHandler.ApplyCheckpoint(cashCheckpoint.Entity, cashTransaction.CashTransactionId);
            }
        }

        protected override bool Validate(CheckpointRequest request)
        {
            return request.Validate();
        }
    }
}