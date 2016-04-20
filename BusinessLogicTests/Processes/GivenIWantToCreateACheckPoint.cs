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
        private RecordCashCheckpointProcess _recordCashCheckpointProcess;

        public GivenIWantToCreateACheckPointForCashTransactions()
        {
            var accountId = 1;
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today;
            var request = new CheckpointRequest(accountId, fromDate, toDate, new List<int>());

            _fakeRepository = new FakeRepository(new FakeDataForCheckpointing());

            _recordCashCheckpointProcess = new RecordCashCheckpointProcess(_fakeRepository, request: request);
            _recordCashCheckpointProcess.Execute();
        }

        [Fact]
        public void ThenICanCreateACheckpoint()
        {
            var checkpoint = _fakeRepository.GetCheckpointByCheckpointId(1);

            Assert.Equal(1, checkpoint.CashCheckpointId);

        }

        //[Fact]
        //public void ThenICanAddTransactionsToACheckpoint()
        //{
        //}

        //[Fact]
        //public void ThenICanFindTransactionsBasedOnACheckpoint()
        //{
        //}

        //Ensure checkpoint opening balance + closing balance = sum of transactions
    }
}
