using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;
using Xunit;

namespace BusinessLogicTests.Processes
{
    public class GivenIWantToCreateACheckPointForCashTransactions
    {
        private readonly FakeCheckpointRepository _fakeCheckpointRepository;
        private readonly FakeCashTransactionRepository _cashTransactionRepository;
        private RecordCashCheckpointProcess _recordCashCheckpointProcess;

        private const int FirstCheckpointId = 1;
        private const int SecondCheckpointId = 2;
        private const int AccountId = 1;
        private const string CheckpointReference = "Checkpoint Reference";

        private readonly DateTime _checkpointStartDate = DateTime.Today;
        private readonly DateTime _checkpointEndtDate = DateTime.Today;

        public GivenIWantToCreateACheckPointForCashTransactions()
        {
            _fakeCheckpointRepository = new FakeCheckpointRepository();
            _cashTransactionRepository = new FakeCashTransactionRepository(new FakeDataForCheckpointing());
        }

        public void CreateCheckpoint(List<CashTransaction> linkedTransactions)
        {
            const int accountId = 1;
            var fromDate = DateTime.Today;
            var toDate = DateTime.Today;
            var itemsToCheckpoint = linkedTransactions.Select(item=>item.MapToDto()).ToList();
            var request = new CheckpointRequest(accountId, fromDate, toDate, itemsToCheckpoint, CheckpointReference);
                        
            _recordCashCheckpointProcess = new RecordCashCheckpointProcess(_fakeCheckpointRepository, _cashTransactionRepository, request);
            _recordCashCheckpointProcess.Execute();
        }

        [Fact]
        public void ThenICanCreateACheckpoint()
        {
            var cashTransactionsForAccount = _cashTransactionRepository.GetCashTransactionsForAccount(1).ToList();

            CreateCheckpoint(cashTransactionsForAccount);

            var checkpoint = _fakeCheckpointRepository.GetCheckpointByCheckpointId(FirstCheckpointId);

            Assert.Equal(FirstCheckpointId, checkpoint.CashCheckpointId);
            Assert.Equal(_checkpointStartDate, checkpoint.FromDate);
            Assert.Equal(_checkpointEndtDate, checkpoint.ToDate);            
            Assert.Equal(CheckpointReference, checkpoint.Reference);
        }

        [Fact]
        public void ThenICanAddTransactionsToACheckpoint()
        {
            CreateCheckpoint(_cashTransactionRepository.GetCashTransactionsForAccount(1).ToList());

            var transaction = _cashTransactionRepository.GetCashTransactionById(1);
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(2);
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(3);
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(4);
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);            
        }

        [Fact]
        public void ThenCheckpointClosingMatchesStartingPlusActuals()
        {
            var accountId = 1;
            CreateCheckpoint(_cashTransactionRepository.GetCashTransactionsForAccount(accountId).Where(tx=>tx.CashTransactionId<3).ToList());

            var transaction = _cashTransactionRepository.GetCashTransactionById(1);
            var runningTotal = transaction.TransactionValue;
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);

            transaction = _cashTransactionRepository.GetCashTransactionById(2);
            runningTotal += transaction.TransactionValue;
            Assert.Equal(FirstCheckpointId, transaction.CheckpointId);

            var checkpoint = _fakeCheckpointRepository.GetCheckpointByCheckpointId(FirstCheckpointId);
            Assert.Equal(runningTotal, checkpoint.ClosingValue);
        }

        [Fact]
        public void WhenTwoCheckpointsAreCreatedThenFirstCheckpointClosingMatchesSecondCheckpointStarting()
        {            
            CreateCheckpoint(_cashTransactionRepository.GetCashTransactionsForAccount(AccountId).Where(tx => tx.CashTransactionId < 3).ToList());

            CreateCheckpoint(_cashTransactionRepository.GetCashTransactionsForAccount(AccountId).Where(tx => tx.CashTransactionId > 2).ToList());
            
            var checkpoint1 = _fakeCheckpointRepository.GetCheckpointByCheckpointId(FirstCheckpointId);
            var checkpoint2 = _fakeCheckpointRepository.GetCheckpointByCheckpointId(SecondCheckpointId);
            Assert.Equal(checkpoint1.ClosingValue, checkpoint2.OpeningValue);
        }        
    }
}
