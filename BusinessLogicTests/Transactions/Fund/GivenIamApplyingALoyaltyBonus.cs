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
    public class GivenIamApplyingALoyaltyBonus
    {
        private readonly FakeRepository _fakeRepository;
        private RecordLoyaltyBonusProcess _process;
        private IFundTransactionHandler _fundTransactionHandler;
        private ICashTransactionHandler _cashTransactionHandler;
        private IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private IInvestmentHandler _investmentHandler;

        private readonly int _accountId = 1;
        readonly decimal _loyaltyBonusAmount = (decimal) 0.84;
        readonly DateTime _transactionDate = DateTime.Now;
        readonly int _existingInvestmentMapId = 1;

        private const int CashTransactionId = 1;

        public GivenIamApplyingALoyaltyBonus()
        {
            _fakeRepository = new FakeRepository();
        }
        private void SetupAndOrExecute(bool execute)
        {
            var request = new InvestmentLoyaltyBonusRequest
            {
                InvestmentMapId = _existingInvestmentMapId,
                Amount = _loyaltyBonusAmount,
                TransactionDate = _transactionDate
            };

            _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            _investmentHandler = new InvestmentHandler(_fakeRepository);
            
            _process = new RecordLoyaltyBonusProcess(
                request,
                _fundTransactionHandler,
                _cashTransactionHandler,
                _accountInvestmentMapProcessor,
                _investmentHandler
                );

            if (execute) _process.Execute();
        }

        [Fact]
        public void TransactionIsValid()
        {
            SetupAndOrExecute(false);
            Assert.True(_process.ProcessValid);
        }


        [Fact]
        public void WhenIRecordALoyaltyTransactionThenAFundTransactionIsRecorded()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);

            Assert.Equal(_existingInvestmentMapId, fundTransaction.InvestmentMapId);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal(FundTransactionTypes.LoyaltyBonus, fundTransaction.TransactionType);
            Assert.Equal(_loyaltyBonusAmount, fundTransaction.TransactionValue);
            Assert.Equal(0, fundTransaction.Quantity);
        }

        [Fact]
        public void WhenIRecordALoyaltyTransactionThenACashDepositIsCreated()
        {
            var source = "Investment 1";
            _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);

            var transaction = _fakeRepository.GetCashTransaction(CashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_loyaltyBonusAmount, transaction.TransactionValue);            
            Assert.Equal(source, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.LoyaltyBonus, transaction.TransactionType);

            Assert.Equal(1, _fakeRepository.GetCashTransactionsForAccount(_accountId).Count());
        }

        [Fact]
        public void WhenIRecordALoyaltyTransactionTheAccountBalanceIsIncreased()
        {
            var accountBeforeBalance = _fakeRepository.GetAccountByAccountId(1).Cash;
            _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
            SetupAndOrExecute(true);
            var accountBeforeAfter = _fakeRepository.GetAccountByAccountId(1).Cash;
            Assert.Equal(accountBeforeBalance + _loyaltyBonusAmount, accountBeforeAfter);
        }



        [Fact]
        public void WhenISellTheSellTransactionAndCommisionTransactionAreLinked()
        {
            var arbitaryId = 1;
            
            SetupAndOrExecute(true);

            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);
            var cashTransaction = _fakeRepository.GetCashTransaction(arbitaryId);

            Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
            Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

            var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        }
    }
}