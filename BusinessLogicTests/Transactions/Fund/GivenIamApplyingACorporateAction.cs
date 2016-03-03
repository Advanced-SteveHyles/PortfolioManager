using System;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Processors.Handlers;
using BusinessLogic.Transactions;
using BusinessLogicTests.FakeRepositories;
using Interfaces;
using PortfolioManager.Constants.TransactionTypes;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.DTO.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund
{
    public class GivenIamApplyingACorporateAction
    {
        private readonly FakeRepository _fakeRepository;
        private RecordCorporateActionTransaction _transaction;
        private IFundTransactionHandler _fundTransactionHandler;
        private ICashTransactionHandler _cashTransactionHandler;
        private IAccountInvestmentMapProcessor _accountInvestmentMapProcessor ;
        private IInvestmentHandler _investmentHandler;

        private readonly int _accountId = 1;
        readonly decimal _corporateActionAmount = (decimal)50;
        readonly DateTime _transactionDate = DateTime.Now;
        readonly int _existingInvestmentMapId = 1;

        public GivenIamApplyingACorporateAction()
        {
            _fakeRepository = new FakeRepository();
        }
        private void SetupAndOrExecute(bool execute)
        {
            var request = new InvestmentCorporateActionRequest
            {
                InvestmentMapId = _existingInvestmentMapId,
                Amount = _corporateActionAmount,
                TransactionDate = _transactionDate
            };

            _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            _investmentHandler  = new InvestmentHandler(_fakeRepository);

            _transaction = new RecordCorporateActionTransaction(
                request, 
                _fundTransactionHandler, 
                _cashTransactionHandler,
                _accountInvestmentMapProcessor,
                _investmentHandler
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
        public void WhenIRecordACorporateActionThenAFundTransactionIsRecorded()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);

            Assert.Equal(_existingInvestmentMapId, fundTransaction.InvestmentMapId);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal("Corporate Action", fundTransaction.TransactionType);
            Assert.Equal(_corporateActionAmount, fundTransaction.TransactionValue);
            Assert.Equal(0, fundTransaction.Quantity);
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnOeicACashTransactionIsCreated()
        {
            _fakeRepository.SetInvestmentClass(_existingInvestmentMapId, PortfolioManager.Constants.Funds.FundClasses.Oeic);
            SetupAndOrExecute(true);

            const int cashTransactionId = 1;
            var transaction = _fakeRepository.GetCashTransaction(cashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_corporateActionAmount, transaction.TransactionValue);
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.CorporateAction, transaction.TransactionType);

            Assert.Equal(1, _fakeRepository.GetCashTransactionsForAccount(_accountId).Count());
        }

        [Fact]
        public void WhenIRecordACorporateActionForAnOeicTheAccountBalanceIsIncreased()
        {
            var accountBeforeBalance =  _fakeRepository.GetAccount(1).Cash;

            _fakeRepository.SetInvestmentClass(_existingInvestmentMapId,
                PortfolioManager.Constants.Funds.FundClasses.Oeic);
            SetupAndOrExecute(true);

            var accountBeforeAfter = _fakeRepository.GetAccount(1).Cash;

            Assert.Equal(accountBeforeBalance + _corporateActionAmount, accountBeforeAfter); 
        }


        [Fact]
        public void WhenIRecordACorporateActionForATrustFundTheAccountBalanceIsNotIncreased()
        {
            var accountBeforeBalance = _fakeRepository.GetAccount(1).Cash;

            _fakeRepository.SetInvestmentClass(_existingInvestmentMapId,
                PortfolioManager.Constants.Funds.FundClasses.Trustfund);
            SetupAndOrExecute(true);

            var accountBeforeAfter = _fakeRepository.GetAccount(1).Cash;
            Assert.Equal(accountBeforeBalance, accountBeforeAfter);
        }

        [Fact]
        public void WhenIRecordACorporateActionForATrustFundCashTransactionIsNotCreated()
        {
            _fakeRepository.SetInvestmentClass(_existingInvestmentMapId, PortfolioManager.Constants.Funds.FundClasses.Trustfund);
            SetupAndOrExecute(true);
            Assert.Equal(0, _fakeRepository.GetCashTransactionsForAccount(_accountId).Count());
        }
    }    
}


