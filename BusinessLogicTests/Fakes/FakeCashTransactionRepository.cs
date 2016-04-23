using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace BusinessLogicTests.Fakes
{
    public class FakeCashTransactionRepository : ICashTransactionRepository
    {
        
        public FakeCashTransactionRepository(FakeData fakeData)
        {
            _fakeData = fakeData;            
        }

        public IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId)
        {
            return _fakeData.CashTransactions().Where(ct => ct.AccountId == accountId).AsQueryable();
        }

        public RepositoryActionResult<CashTransaction> ApplyCheckpoint(CashCheckpoint cashCheckpoint, int transactionId)
        {
            var transaction = GetCashTransactionById(transactionId);
            _fakeData.CashTransactions().Remove(transaction);
            transaction.CheckpointId = cashCheckpoint.CashCheckpointId;
            _fakeData.CashTransactions().Add(transaction);

            return new RepositoryActionResult<CashTransaction>(transaction, RepositoryActionStatus.Updated);
        }

        public CashTransaction GetCashTransactionById(int cashTransactionId)
        {
            return _fakeData.CashTransactions().Single(t => t.CashTransactionId == cashTransactionId);
        }

        private int _nextCashTransactionId;
        private FakeData _fakeData;

        public RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request)
        {
            _nextCashTransactionId++;
            var cashTransaction = new CashTransaction()
            {
                CashTransactionId = _nextCashTransactionId,
                AccountId = request.AccountId,
                TransactionDate = request.TransactionDate,
                TransactionValue = request.TransactionValue,
                Source = request.Source,
                IsTaxRefund = request.IsTaxRefund,
                TransactionType = request.TransactionType,
                LinkedTransactionType = request.LinkedTransactionType,
                LinkedTransaction = request.LinkedTransaction
            };
            _fakeData.CashTransactions().Add(
                cashTransaction
                );

            return null;
        }

    }
}