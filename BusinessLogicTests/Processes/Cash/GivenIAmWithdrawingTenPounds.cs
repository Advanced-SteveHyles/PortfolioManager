using System;
using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Processes.Cash
{
    public class GivenIAmWithdrawingTenPounds
    {
        private BaseProcess<WithdrawalTransactionRequest> _withdrawalTransaction;
        private readonly FakeRepository _fakeRepository;
        private readonly FakeCashTransactionRepository _fakeCashTransactionRepository;
        private readonly CashTransactionHandler _cashTransactionHandler;
        const int AccountId = 1;
        const int TransactionValue = 10;
        const int ArbitaryId = 1;
        readonly DateTime transactionDate = DateTime.Now;
        const string Source = "Test";

        public GivenIAmWithdrawingTenPounds()
        {
            _fakeRepository = new FakeRepository(new FakeDataGeneric());
            _fakeCashTransactionRepository = new FakeCashTransactionRepository(new FakeDataGeneric());
            _cashTransactionHandler = new CashTransactionHandler(_fakeCashTransactionRepository, _fakeRepository);                                   
        }

        private void MakeRequest(string transactionType)
        {
            var withdrawalTransactionRequest = new WithdrawalTransactionRequest()
            {
                AccountId = AccountId,
                Value = TransactionValue,
                Source = Source,
                TransactionDate = transactionDate,
                TransactionType = transactionType
            };

            _withdrawalTransaction = new RecordWithdrawalProcess(withdrawalTransactionRequest, _cashTransactionHandler);
        }

        public void ValidTransactionCanExecute()
        {
            MakeRequest(CashWithdrawalTransactionTypes.Withdrawal);
            _withdrawalTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);            
            Assert.Equal(-TransactionValue, account.Cash);
        }
        

        [Fact]
        public void WhenTheTransactionCompletesThereIsARecordOfTheDeposit()
        {
            const bool isTaxRefund = false;
            MakeRequest(CashWithdrawalTransactionTypes.Withdrawal);
            _withdrawalTransaction.Execute();

            var transaction = _fakeCashTransactionRepository.GetCashTransactionById(ArbitaryId);
            
            Assert.Equal(AccountId, transaction.AccountId);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(-TransactionValue, transaction.TransactionValue);
            Assert.Equal(Source, transaction.Source);
            
            Assert.Equal(isTaxRefund, transaction.IsTaxRefund);            
        }

        [Fact]
        public void WhenTheTransactionCompletesThereAccountBalanceIsCorrect()
        {
            MakeRequest(CashWithdrawalTransactionTypes.Withdrawal);
            _withdrawalTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(-TransactionValue, account.Cash);
        }


        [Theory]
        [MemberData("GetData")]
        public void WhenTheTransactionCompletesTheTransactionTypeMatches(string requestedType)
        {
            MakeRequest(requestedType);
            _withdrawalTransaction.Execute();
            var transaction = _fakeCashTransactionRepository.GetCashTransactionById(ArbitaryId);

            Assert.Equal(requestedType, transaction.TransactionType);
        }

        public static IEnumerable<object> GetData => new object[]
       {
          new object[] { CashWithdrawalTransactionTypes.Withdrawal},
          new object[] { CashWithdrawalTransactionTypes.Fees}
       };
    }
}