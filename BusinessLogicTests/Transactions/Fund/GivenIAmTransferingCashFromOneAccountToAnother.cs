using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund
{
    public class GivenIAmTransferingCashFromOneAccountToAnother
    {
        private readonly FakeRepository _fakeRepository;
        private RecordCashTransferTransaction _transaction;
        private IFundTransactionHandler _fundTransactionHandler;
        private ICashTransactionHandler _cashTransactionHandler;
        private IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private IInvestmentHandler _investmentHandler;

        private readonly int _accountId1 = 1;
        private readonly int _accountId2 = 2;
        readonly decimal _transferAmount = (decimal)75.69;
        readonly DateTime _transactionDate = DateTime.Now;
        
        private const int CashTransactionId = 1;

        public GivenIAmTransferingCashFromOneAccountToAnother()
        {
            _fakeRepository = new FakeRepository();
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

            _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            _investmentHandler = new InvestmentHandler(_fakeRepository);

            _transaction = new RecordCashTransferTransaction(
                request,                
                _cashTransactionHandler                                
                );

            if (execute) _transaction.Execute();
        }

        [Fact]
        public void TransactionIsValid()
        {
            SetupAndOrExecute(false);
            Assert.True(_transaction.CommandValid);
        }

        [Fact]
        public void WhenIRecordATransferAACashDepositIsCreated()
        {
            SetupAndOrExecute(true);

            var transaction = _fakeRepository.GetCashTransaction(CashTransactionId);
            Assert.Equal(_accountId1, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_transferAmount, transaction.TransactionValue);
            Assert.Equal("Account 2", transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CashTransfer, transaction.TransactionType);
            Assert.Equal(1, _fakeRepository.GetCashTransactionsForAccount(_accountId1).Count());
        }


        [Fact]
        public void WhenIRecordATransferAACashWithdrawalIsCreated()
        {
            SetupAndOrExecute(true);

            var transaction = _fakeRepository.GetCashTransaction(CashTransactionId);
            Assert.Equal(_accountId1, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_transferAmount, transaction.TransactionValue);
            Assert.Equal("Account 1", transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CashTransfer, transaction.TransactionType);

            Assert.Equal(1, _fakeRepository.GetCashTransactionsForAccount(_accountId1).Count());
        }

        
        //[Fact]
        //public void WhenIRecordALoyaltyTransactionTheAccountBalanceIsIncreased()
        //{
        //    var accountBeforeBalance = _fakeRepository.GetAccountByAccountId(1).Cash;
        //    _fakeRepository.SetInvestmentIncome(_existingInvestmentMapId, FundIncomeTypes.Income);
        //    SetupAndOrExecute(true);
        //    var accountBeforeAfter = _fakeRepository.GetAccountByAccountId(1).Cash;
        //    Assert.Equal(accountBeforeBalance + _transferAmount, accountBeforeAfter);
        //}


        //[Fact]
        //public void WhenISellTheSellTransactionAndCommisionTransactionAreLinked()
        //{
        //    var arbitaryId = 1;
        //    const int commissionId = 2;

        //    SetupAndOrExecute(true);

        //    var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);

        //    var cashTransaction = _fakeRepository.GetCashTransaction(commissionId);

        //    Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
        //    Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
        //    Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

        //    var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
        //    Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
        //    Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        //}
    }
}