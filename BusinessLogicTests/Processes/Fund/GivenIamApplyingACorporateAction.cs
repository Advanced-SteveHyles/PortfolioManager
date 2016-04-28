using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
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
    public class GivenIamApplyingACorporateAction
    {
        private readonly FakeInvestmentRepository _fakeInvestmentRepository;
        private RecordCorporateActionProcess _process;
        private FundTransactionHandler _fundTransactionHandler;
        private CashTransactionHandler _cashTransactionHandler;
        private AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private InvestmentHandler _investmentHandler;

        private readonly int _accountId = 1;
        readonly decimal _corporateActionAmount = 50;
        readonly DateTime _transactionDate = DateTime.Now;
        private readonly FakeCashTransactionRepository _cashTransactionRepository;

        private const int FundTransactionId = 1;
        private const int CashTransactionId = 1;

        public GivenIamApplyingACorporateAction()
        {
            _fakeInvestmentRepository = new FakeInvestmentRepository(new FakeDataGeneric());
            _cashTransactionRepository = new FakeCashTransactionRepository(new FakeDataGeneric());
        }
        private void SetupAndOrExecute(bool execute)
        {
            var request = new InvestmentCorporateActionRequest
            {
                InvestmentMapId = FakeDataGeneric.FakeInvestmentId,
                Amount = _corporateActionAmount,
                TransactionDate = _transactionDate
            };

            _fundTransactionHandler = new FundTransactionHandler(_fakeInvestmentRepository);
            _cashTransactionHandler = new CashTransactionHandler(_cashTransactionRepository, _fakeInvestmentRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeInvestmentRepository);
            _investmentHandler = new InvestmentHandler(_fakeInvestmentRepository);

            _process = new RecordCorporateActionProcess(
                request,
                _fundTransactionHandler,
                _cashTransactionHandler,
                _accountInvestmentMapProcessor,
                _investmentHandler
                );

            if (execute) _process.Execute();
        }
        

        [Fact]
        public void WhenIRecordACorporateActionThenAFundTransactionIsRecorded()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Accumulation);
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);

            Assert.Equal(FakeDataGeneric.FakeInvestmentId, fundTransaction.InvestmentMapId);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal(FundTransactionTypes.CorporateAction, fundTransaction.TransactionType);
            Assert.Equal(_corporateActionAmount, fundTransaction.TransactionValue);
            Assert.Equal(0, fundTransaction.Quantity);
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnIncomeFundACashRefundIsCreated()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var transaction = _cashTransactionRepository.GetCashTransactionById(CashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_corporateActionAmount, transaction.TransactionValue);
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CorporateAction, transaction.TransactionType);

            Assert.Equal(1, _cashTransactionRepository.GetCashTransactionsForAccount(_accountId).Count());
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnIncomeFundTheAccountBalanceIsIncreased()
        {
            var accountBeforeBalance = _fakeInvestmentRepository.GetAccountByAccountId(1).Cash;
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);
            var accountBeforeAfter = _fakeInvestmentRepository.GetAccountByAccountId(1).Cash;
            Assert.Equal(accountBeforeBalance + _corporateActionAmount, accountBeforeAfter);
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnAccumulationFundTheAccountBalanceIsNotIncreased()
        {
            var accountBeforeBalance = _fakeInvestmentRepository.GetAccountByAccountId(1).Cash;

            _fakeInvestmentRepository.SetInvestmentClass(FakeDataGeneric.FakeInvestmentId, FundClasses.UnitTrust);
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Accumulation);
            SetupAndOrExecute(true);

            var accountBeforeAfter = _fakeInvestmentRepository.GetAccountByAccountId(1).Cash;
            Assert.Equal(accountBeforeBalance, accountBeforeAfter);
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnAccumulationFundCashTransactionIsNotCreated()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Accumulation);
            SetupAndOrExecute(true);
            Assert.Equal(0, _cashTransactionRepository.GetCashTransactionsForAccount(_accountId).Count());
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnIncomeFundTheFundTransactionIsCorrect()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var transaction = _fakeInvestmentRepository.GetFundTransaction(FundTransactionId);
            Assert.Equal(FundTransactionTypes.ReturnOfCapital, transaction.TransactionType);
        }


        [Fact]
        public void WhenIRecordACorporateActionForAnAccumulationFundTheFundTransactionIsCorrect()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Accumulation);
            SetupAndOrExecute(true);

            var transaction = _fakeInvestmentRepository.GetFundTransaction(FundTransactionId);
            Assert.Equal(FundTransactionTypes.CorporateAction, transaction.TransactionType);
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnAccumulationThereIsNoLinkedTransaction()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Accumulation);

            SetupAndOrExecute(true);

            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(FundTransactionId);
           
            Assert.Equal(null, fundTransaction.LinkedTransaction);
            Assert.True(string.IsNullOrEmpty(fundTransaction.LinkedTransactionType));            
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnIncomeFundThenALinkedTransactionExists()
        {
            _fakeInvestmentRepository.SetInvestmentIncome(FakeDataGeneric.FakeInvestmentId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(FundTransactionId);
            var cashTransaction = _cashTransactionRepository.GetCashTransactionById(CashTransactionId);

            Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
            Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

            var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        }

    }
}