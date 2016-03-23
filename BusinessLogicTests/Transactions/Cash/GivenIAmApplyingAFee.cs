using System;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Cash
{
    public class GivenIAmApplyingAFee
    {
        private readonly ICommandRunner _feeTransaction;
        private readonly FakeRepository _fakeRepository;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        const int AccountId = 1;
        const decimal TransactionValue = (decimal)34.39;
        const int ArbitaryId = 1;
        readonly DateTime transactionDate = DateTime.Now;
        
        public GivenIAmApplyingAFee()
        {
            _fakeRepository = new FakeRepository();
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);


            var feeTransactionRequest = new FeeTransactionRequest()
            {
                AccountId = AccountId,
                Value = TransactionValue,                
                TransactionDate = transactionDate,
            };

            _feeTransaction = new RecordFeeTransaction(feeTransactionRequest, _cashTransactionHandler);
        }

        [Fact]
        public void ValidTransactionCanExecute()
        {
            Assert.True(_feeTransaction.CommandValid);

            _feeTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(-TransactionValue, account.Cash);
        }


        [Fact]
        public void WhenTheTransactionCompletesThereIsARecordOfTheDeposit()
        {
            const bool isTaxRefund = false;

            _feeTransaction.Execute();

            var transaction = _fakeRepository.GetCashTransaction(ArbitaryId);

            Assert.Equal(AccountId, transaction.AccountId);
            Assert.Equal(transactionDate, transaction.TransactionDate);
            Assert.Equal(TransactionValue, transaction.TransactionValue);
            
            Assert.Equal(isTaxRefund, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Fees, transaction.TransactionType);
        }

        [Fact]
        public void WhenTheTransactionCompletesThereAccountBalanceIsCorrect()
        {
            _feeTransaction.Execute();
            var account = _fakeRepository.GetAccountByAccountId(ArbitaryId);
            Assert.Equal(-TransactionValue, account.Cash);
        }
    }
}