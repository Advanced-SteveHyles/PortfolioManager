using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund
{
    public class GivenIAmTransferingCashFromOneAccountToAnother
    {
        private readonly FakeInvestmentRepository _fakeInvestmentRepository;
        private readonly FakeCashTransactionRepository _fakeCashTransactionRepository;
        private RecordCashTransferProcess _process;
        private CashTransactionHandler _cashTransactionHandler;
        private AccountHandler _accountHandler;

        private readonly int _accountId1 = 1;
        private readonly int _accountId2 = 2;
        private readonly decimal _transferAmount = (decimal)75.69;
        private readonly DateTime _transactionDate = DateTime.Now;

        public GivenIAmTransferingCashFromOneAccountToAnother()
        {
            _fakeInvestmentRepository = new FakeInvestmentRepository(new FakeDataGeneric());
            _fakeCashTransactionRepository = new FakeCashTransactionRepository(new FakeDataGeneric());
        }
        private void SetupAndOrExecute(bool execute)
        {
            var request = new CashTransferRequest
            {
                FromAccount = _accountId1,
                ToAccount = _accountId2,
                Amount = _transferAmount,
                TransactionDate = _transactionDate
            };

            _cashTransactionHandler = new CashTransactionHandler(_fakeCashTransactionRepository, _fakeInvestmentRepository);
            _accountHandler = new AccountHandler(_fakeInvestmentRepository);

            _process = new RecordCashTransferProcess(
                request,
                _cashTransactionHandler,
                _accountHandler
                );

            if (execute) _process.Execute();
        }
        
        [Fact]
        public void WhenIRecordATransferAWithdrawalIsRecorded()
        {
            SetupAndOrExecute(true);

            const int cashTransactionId = 1;
            var transaction2 = _fakeCashTransactionRepository.GetCashTransactionById(cashTransactionId);
            var withdrawalAmount = -_transferAmount;
            Assert.Equal(_accountId1, transaction2.AccountId);
            Assert.Equal(_transactionDate, transaction2.TransactionDate);
            Assert.Equal(withdrawalAmount, transaction2.TransactionValue);
            Assert.Equal("TFR Acc1 => Acc2", transaction2.Source);
            Assert.Equal(false, transaction2.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CashTransferOut, transaction2.TransactionType);
            Assert.Equal(1, _fakeCashTransactionRepository.GetCashTransactionsForAccount(_accountId1).Count());
        }

        [Fact]
        public void WhenIRecordATransferADepostIsRecorded()
        {
            SetupAndOrExecute(true);

            const int cashTransactionId = 2;
            var transaction1 = _fakeCashTransactionRepository.GetCashTransactionById(cashTransactionId);
            Assert.Equal(_accountId2, transaction1.AccountId);
            Assert.Equal(_transactionDate, transaction1.TransactionDate);
            Assert.Equal(_transferAmount, transaction1.TransactionValue);
            Assert.Equal("TFR Acc1 => Acc2", transaction1.Source);
            Assert.Equal(false, transaction1.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CashTransferIn, transaction1.TransactionType);
            Assert.Equal(1, _fakeCashTransactionRepository.GetCashTransactionsForAccount(_accountId2).Count());
        }

        [Fact]
        public void WhenIRecordATransferTheTransactionsAreLinked()
        {
            SetupAndOrExecute(true);
            var transaction1 = _fakeCashTransactionRepository.GetCashTransactionById(1);
            var transaction2 = _fakeCashTransactionRepository.GetCashTransactionById(2);
            Assert.NotEqual(Guid.Empty, transaction1.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, transaction2.LinkedTransaction);
            Assert.Equal(transaction1.LinkedTransaction, transaction2.LinkedTransaction);

            var linkedTransactionType = TransactionLink.CashToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, transaction1.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, transaction2.LinkedTransactionType);
        }

        [Fact]
        public void WhenIRecordATransferTheFromAccountBalanceIsDecreased()
        {
            var accountBeforeBalance = _fakeInvestmentRepository.GetAccountByAccountId(_accountId1).Cash;
            SetupAndOrExecute(true);
            var accountBeforeAfter = _fakeInvestmentRepository.GetAccountByAccountId(_accountId1).Cash;
            Assert.Equal(accountBeforeBalance - _transferAmount, accountBeforeAfter);
        }

        [Fact]
        public void WhenIRecordATransferTheToAccountBalanceIsIncreased()
        {
            var accountBeforeBalance = _fakeInvestmentRepository.GetAccountByAccountId(_accountId2).Cash;
            SetupAndOrExecute(true);
            var accountBeforeAfter = _fakeInvestmentRepository.GetAccountByAccountId(_accountId2).Cash;
            Assert.Equal(accountBeforeBalance + _transferAmount, accountBeforeAfter);
        }
    }
}