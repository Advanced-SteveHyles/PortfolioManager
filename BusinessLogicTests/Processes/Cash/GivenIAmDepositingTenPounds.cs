using System;
using System.Collections.Generic;
using System.Dynamic;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Cash
{

    public class GivenIAmDepositingTenPounds
    {
        private BaseProcess<DepositTransactionRequest> _depositTransaction;
        private readonly FakeRepository _fakeRepository;
        const int AccountId = 1;
        const int TransactionValue = 10;
        const int ArbitaryId = 1;
        readonly DateTime transactionDate = DateTime.Now;
        private ICashTransactionHandler _cashTransactionHandler;
        const string Source = "Test";

        public GivenIAmDepositingTenPounds()
        {
            _fakeRepository = new FakeRepository(new FakeData());
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
        }

        private void MakeRequest(string transactionType)
        {
            var depositTransactionRequest = new DepositTransactionRequest
            {
                AccountId = AccountId,
                Value = TransactionValue,
                Source = Source,
                TransactionDate = transactionDate,
                TransactionType = transactionType
            };

            _depositTransaction = new RecordDepositProcess(depositTransactionRequest, _cashTransactionHandler, null);
            
        }

        [Fact]
        public void ValidTransactionCanExecute()
        {
            MakeRequest(CashDepositTransactionTypes.Deposit);
            _depositTransaction.Execute();

            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(TransactionValue, account.Cash);
        }

        [Fact]
        public void WhenTheTransactionCompletesThereIsARecordOfTheDeposit()
        {
            const bool isTaxRefund = false;
            MakeRequest(CashDepositTransactionTypes.Deposit);
            _depositTransaction.Execute();

            var transaction = _fakeRepository.GetCashTransaction(ArbitaryId);

            Assert.Equal(AccountId, transaction.AccountId);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(TransactionValue, transaction.TransactionValue);
            Assert.Equal(Source, transaction.Source);
            Assert.Equal(isTaxRefund, transaction.IsTaxRefund);         
        }

        [Fact]
        public void WhenTheTransactionCompletesThereAccountBalanceIsCorrect()
        {
            MakeRequest(CashDepositTransactionTypes.Deposit);
            _depositTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(TransactionValue, account.Cash);
        }

        [Theory]
        [MemberData("GetData")]
        public void WhenTheTransactionCompletesTheTransactionTypeMatches(string requestedType)
        {
            MakeRequest(requestedType);
            _depositTransaction.Execute();
            var transaction = _fakeRepository.GetCashTransaction(ArbitaryId);

            Assert.Equal(requestedType, transaction.TransactionType);
        }

        public static IEnumerable<object> GetData => new object[]
       {
          new object[] { CashDepositTransactionTypes.Deposit},
          new object[] { CashDepositTransactionTypes.InterestPaid}
       };

    }
}

