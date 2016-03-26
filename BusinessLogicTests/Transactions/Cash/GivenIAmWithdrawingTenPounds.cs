using System;
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
    public class GivenIAmWithdrawingTenPounds
    {
        private readonly BaseProcess<WithdrawalTransactionRequest> _withdrawalTransaction;
        private readonly FakeRepository _fakeRepository;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        const int AccountId = 1;
        const int TransactionValue = 10;
        const int ArbitaryId = 1;
        DateTime transactionDate = DateTime.Now;
        const string Source = "Test";

        public GivenIAmWithdrawingTenPounds()
        {
            _fakeRepository = new FakeRepository();
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            
            
            var withdrawalTransactionRequest = new WithdrawalTransactionRequest()
            {
                AccountId = AccountId,
                Value = TransactionValue,
                Source = Source,
                TransactionDate = transactionDate,                
            };

            _withdrawalTransaction = new  RecordWithdrawalProcess(withdrawalTransactionRequest, _cashTransactionHandler);
        }

        [Fact]
        public void ValidTransactionCanExecute()
        {
            _withdrawalTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);            
            Assert.Equal(-TransactionValue, account.Cash);
        }
        

        [Fact]
        public void WhenTheTransactionCompletesThereIsARecordOfTheDeposit()
        {
            const bool isTaxRefund = false;

            _withdrawalTransaction.Execute();

            var transaction = _fakeRepository.GetCashTransaction(ArbitaryId);
            
            Assert.Equal(AccountId, transaction.AccountId);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(-TransactionValue, transaction.TransactionValue);
            Assert.Equal(Source, transaction.Source);
            
            Assert.Equal(isTaxRefund, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Withdrawal, transaction.TransactionType);
        }

        [Fact]
        public void WhenTheTransactionCompletesThereAccountBalanceIsCorrect()
        {
            _withdrawalTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(-TransactionValue, account.Cash);
        }
    }
}