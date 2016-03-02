using System;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Transactions;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using PortfolioManager.Constants.TransactionTypes;
using PortfolioManager.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Cash
{

    public class GivenIAmDepositingTenPounds
    {
        private readonly ICommandRunner _depositTransaction;
        private readonly FakeRepository _fakeRepository;
        const int AccountId = 1;
        const int TransactionValue = 10;
        const int ArbitaryId = 1;
        DateTime transactionDate = DateTime.Now;
        const string Source = "Test";

        public GivenIAmDepositingTenPounds()
        {
            _fakeRepository = new FakeRepository();
            ICashTransactionHandler cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);

            var depositTransactionRequest = new DepositTransactionRequest
            {
                AccountId = AccountId,
                Value = TransactionValue,
                Source = Source,
                TransactionDate = transactionDate
            };

            _depositTransaction = new RecordDepositTransaction(depositTransactionRequest, cashTransactionHandler);
        }

        [Fact]
        public void ValidTransactionCanExecute()
        {
            Assert.True(_depositTransaction.CommandValid);

            _depositTransaction.Execute();
            var account = _fakeRepository.GetAccount(ArbitaryId);
            Assert.Equal(TransactionValue, account.Cash);
        }

        [Fact]
        public void WhenTheTransactionCompletesThereIsARecordOfTheDeposit()
        {
            _depositTransaction.Execute();

            const bool isTaxRefund = false;

            _depositTransaction.Execute();

            var transaction = _fakeRepository.GetCashTransaction(ArbitaryId);

            Assert.Equal(AccountId, transaction.AccountId);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(TransactionValue, transaction.TransactionValue);
            Assert.Equal(Source, transaction.Source);
            Assert.Equal(isTaxRefund, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Deposit, transaction.TransactionType);

        }

        [Fact]
        public void WhenTheTransactionCompletesThereAccountBalanceIsCorrect()
        {
            _depositTransaction.Execute();
            var account = _fakeRepository.GetAccount(ArbitaryId);
            Assert.Equal(TransactionValue, account.Cash);
        }
    }
}

