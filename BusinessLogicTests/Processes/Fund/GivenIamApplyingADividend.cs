using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund
{
    public class GivenIamApplyingADividend
    {
        private readonly FakeRepository _fakeRepository;
        private RecordDividendProcess _process;
        private IFundTransactionHandler _fundTransactionHandler;
        private ICashTransactionHandler _cashTransactionHandler;
        private IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;

        private readonly int _accountId = 1;
        readonly decimal _dividentAmount = 50;
        readonly DateTime _transactionDate = DateTime.Now;
        readonly int _existingInvestmentMapId = 1;

        private const int FundTransactionId = 1;
        private const int CashTransactionId = 1;

        public GivenIamApplyingADividend()
        {
            _fakeRepository = new FakeRepository(new FakeData());
        }
        private void SetupAndOrExecute(bool execute)
        {
            var request = new InvestmentDividendRequest
            {
                InvestmentMapId = _existingInvestmentMapId,
                Amount = _dividentAmount,
                TransactionDate = _transactionDate
            };

            _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            new InvestmentHandler(_fakeRepository);

            _process = new RecordDividendProcess(
                request,
                _fundTransactionHandler,
                _cashTransactionHandler,
                _accountInvestmentMapProcessor
                );

            if (execute) _process.Execute();
        }


        [Fact]
        public void WhenIRecordADividendThenAFundTransactionIsRecorded()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);

            Assert.Equal(_existingInvestmentMapId, fundTransaction.InvestmentMapId);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal(FundTransactionTypes.Dividend, fundTransaction.TransactionType);
            Assert.Equal(_dividentAmount, fundTransaction.TransactionValue);
            Assert.Equal(0, fundTransaction.Quantity);
        }

        [Fact]
        public void WhenIRecordADividendACashRefundIsCreated()
        {
            _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var transaction = _fakeRepository.GetCashTransaction(CashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_dividentAmount, transaction.TransactionValue);
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Dividend, transaction.TransactionType);

            Assert.Equal(1, _fakeRepository.GetCashTransactionsForAccount(_accountId).Count());
        }

        [Fact]
        public void WhenIRecordADividendTheAccountBalanceIsIncreased()
        {
            var accountBeforeBalance = _fakeRepository.GetAccountByAccountId(1).Cash;
            _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);
            var accountBeforeAfter = _fakeRepository.GetAccountByAccountId(1).Cash;
            Assert.Equal(accountBeforeBalance + _dividentAmount, accountBeforeAfter);
        }
        
        [Fact]
        public void WhenIRecordADividendThenALinkedTransactionExists()
        {
            _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var fundTransaction = _fakeRepository.GetFundTransaction(FundTransactionId);
            var cashTransaction = _fakeRepository.GetCashTransaction(CashTransactionId);

            Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
            Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

            var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        }

    }
}