using System;
using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Repositories;
using Xunit;

namespace BusinessLogicTests.Processes
{
    public class GivenIWantToCreateACheckPointForCashTransactions
    {
        private FakeCheckpointRepository _fakeCheckpointRepository;
        private FakeCashTransactionRepository _cashTransactionRepository;
        private RecordCashCheckpointProcess _recordCashCheckpointProcess;
        
        const int firstCheckpointId = 1;

        readonly DateTime checkpointStartDate = DateTime.Today;
        readonly DateTime checkpointEndtDate = DateTime.Today;

        public void Setup(List<int> linkedTransactions)
        {
            var accountId = 1;
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today;
            var request = new CheckpointRequest(accountId, fromDate, toDate, linkedTransactions);
            
            _fakeCheckpointRepository = new FakeCheckpointRepository();
            _cashTransactionRepository = new FakeCashTransactionRepository(new FakeDataForCheckpointing());

            _recordCashCheckpointProcess = new RecordCashCheckpointProcess(_fakeCheckpointRepository, _cashTransactionRepository, request);
            _recordCashCheckpointProcess.Execute();
        }

        [Fact]
        public void ThenICanCreateACheckpoint()
        {
            Setup(new List<int>());
            var checkpoint = _fakeCheckpointRepository.GetCheckpointByCheckpointId(firstCheckpointId);

            Assert.Equal(firstCheckpointId, checkpoint.CashCheckpointId);
            Assert.Equal(checkpointStartDate, checkpoint.FromDate);
            Assert.Equal(checkpointEndtDate, checkpoint.ToDate);
        }

        [Fact]
        public void ThenICanAddTransactionsToACheckpoint()
        {
            Setup(new List<int>(){{ 1},{2},{ 3},{ 4}});

            var transaction = _cashTransactionRepository.GetCashTransactionById(1);
            Assert.Equal(firstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(2);
            Assert.Equal(firstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(3);
            Assert.Equal(firstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(4);
            Assert.Equal(firstCheckpointId, transaction.CheckpointId);            
        }

        //[Fact]
        //public void ThenICanFindTransactionsBasedOnACheckpoint()
        //{
        //}

        //Ensure checkpoint opening balance + closing balance = sum of transactions

        //Ensure once checkpoint is present, new transaction date cannot be before checkpoint
    }
}
