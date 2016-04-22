using System;
using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Xunit;

namespace BusinessLogicTests.Processes
{
    public class GivenIWantToCreateACheckPointForCashTransactions
    {
        private FakeRepository _fakeRepository;
        private FakeCheckpointRepository _fakeCheckpointRepository;
        private CashTransactionRepository _fakeTransactionRepository;
        private RecordCashCheckpointProcess _recordCashCheckpointProcess;
        
        const int firstCheckpointId = 1;

        DateTime checkpointStartDate = DateTime.Today;
        DateTime checkpointEndtDate = DateTime.Today;

        public void Setup(List<int> linkedTransactions)
        {
            var accountId = 1;
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today;
            var request = new CheckpointRequest(accountId, fromDate, toDate, new List<int>());

            _fakeRepository = new FakeRepository(new FakeDataForCheckpointing());
            _fakeCheckpointRepository = new FakeCheckpointRepository();
            _recordCashCheckpointProcess = new RecordCashCheckpointProcess(_fakeCheckpointRepository, _fakeRepository, request);
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

            var transaction = _fakeTransactionRepository.getTransaction(1);
            transaction = _fakeTransactionRepository.getTransaction(2);
            transaction = _fakeTransactionRepository.getTransaction(3);
            transaction = _fakeTransactionRepository.getTransaction(4);

            var checkpoint = _fakeCheckpointRepository.GetCheckpointByCheckpointId(firstCheckpointId);

        }

        //[Fact]
        //public void ThenICanFindTransactionsBasedOnACheckpoint()
        //{
        //}

        //Ensure checkpoint opening balance + closing balance = sum of transactions

        //Ensure once checkpoint is present, new transaction date cannot be before checkpoint
    }
}
