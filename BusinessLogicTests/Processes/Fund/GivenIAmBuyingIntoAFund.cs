using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using ExtensionMethods;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Transactions.Fund
{
    public class GivenIAmBuyingIntoAFund
    {
        private decimal _numberOfShares;
        private decimal _priceOfOneShare;
        private decimal _commission;
        private decimal _valueOfTransaction;
        private DateTime _transactionDate;
        private readonly FakeInvestmentRepository _fakeInvestmentRepository = new FakeInvestmentRepository(new FakeDataGeneric());
        private RecordFundBuyProcess _buyProcess;
        private int _accountId;

        private AccountHandler _accountHandler;
        private CashTransactionHandler _cashCashTransactionHandler;
        private AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private FundTransactionHandler _fundTransactionHandler;
        private InvestmentHandler _investmentHandler;

        private DateTime _settlementDate;
        private PriceHistoryHandler _priceHistoryHandler;
        private int _existingInvestmentMapId = 1;
        public ICashTransactionRepository _CashTransactionRepository { get; }

        public GivenIAmBuyingIntoAFund()
        {
            _CashTransactionRepository = new FakeCashTransactionRepository(new FakeDataGeneric());
        }

        private void SetupAndOrExecute(bool execute)
        {

            _numberOfShares = 10;
            _priceOfOneShare = 1;
            _commission = 2;
            _valueOfTransaction = (_numberOfShares * _priceOfOneShare) + _commission;
            _transactionDate = DateTime.Now;
            _settlementDate = DateTime.Today.AddDays(14);
            _accountId = 1;

            var request = new InvestmentBuyRequest
            {
                InvestmentMapId = _existingInvestmentMapId,
                Quantity = _numberOfShares,
                BuyPrice = _priceOfOneShare,
                PurchaseDate = _transactionDate,
                SettlementDate = _settlementDate,
                UpdatePriceHistory = false,
                Value = _valueOfTransaction,
                Charges = _commission
            };

            _accountHandler = new AccountHandler(_fakeInvestmentRepository);
            _cashCashTransactionHandler = new CashTransactionHandler(_CashTransactionRepository, _fakeInvestmentRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeInvestmentRepository);
            _fundTransactionHandler = new FundTransactionHandler(_fakeInvestmentRepository);
            _priceHistoryHandler = new PriceHistoryHandler(_fakeInvestmentRepository);
            _investmentHandler = new InvestmentHandler(_fakeInvestmentRepository);

            _buyProcess = new RecordFundBuyProcess(request, _accountHandler,
                        _cashCashTransactionHandler, _accountInvestmentMapProcessor,
                        _fundTransactionHandler, _priceHistoryHandler,
                        _investmentHandler);

            if (execute) _buyProcess.Execute();
        }

        [Fact]
        public void WhenIBuyThenTheAccountValueIsReduced()
        {
            SetupAndOrExecute(true);

            var account = _fakeInvestmentRepository.GetAccountByAccountId(_accountId);
            Assert.Equal(-_valueOfTransaction - _commission, account.Cash);
        }

        [Fact]
        public void WhenIBuyThenTheAccountHasARecordOfTheWithdrawal()
        {
            SetupAndOrExecute(true);

            var cashTransactionId = 1;
            var transaction = _CashTransactionRepository.GetCashTransactionById(cashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_valueOfTransaction, transaction.TransactionValue.Negate());
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.FundPurchase, transaction.TransactionType);
        }

        [Fact]
        public void WhenIBuyThenTheAccountHasARecordOfTheCommision()
        {
            SetupAndOrExecute(true);

            var cashTransactionId = 2;
            var transaction = _CashTransactionRepository.GetCashTransactionById(cashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_commission, transaction.TransactionValue.Negate());
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Commission, transaction.TransactionType);
        }

        [Fact]
        public void WhenIBuyThenTheShareCountIsIncreased()
        {
            SetupAndOrExecute(true);

            var accountFundMap = _fakeInvestmentRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            var startingNumberOfShares = 10;
            Assert.Equal(_numberOfShares + startingNumberOfShares, accountFundMap.Quantity);
        }

        [Fact]
        public void WhenIBuyThenTheFundTransactionIsCorrect()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);
            Assert.Equal(_priceOfOneShare, fundTransaction.BuyPrice);
            Assert.Equal(FundTransactionTypes.Buy, fundTransaction.TransactionType);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal(_settlementDate, fundTransaction.SettlementDate);
            Assert.Equal(string.Empty, fundTransaction.Source);
            Assert.Equal(_numberOfShares, fundTransaction.Quantity);
            Assert.Equal(null, fundTransaction.SellPrice);
            Assert.Equal(_priceOfOneShare, fundTransaction.BuyPrice);
            Assert.Equal(_commission, fundTransaction.Charges);

            var transactionValue = (_numberOfShares * _priceOfOneShare) + _commission;
            Assert.Equal(transactionValue, fundTransaction.TransactionValue);
        }

        [Fact]
        public void WhenIBuyThenTheFundTransactionAndTheCashTransactionValueAreIdenticalButOpposite()
        {
            SetupAndOrExecute(true);
            _buyProcess.Execute();

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);
            var cashTransaction = _CashTransactionRepository.GetCashTransactionById(arbitaryId);

            var cashValue = cashTransaction.TransactionValue;
            Assert.Equal(fundTransaction.TransactionValue, cashValue.Negate());
        }
        
        [Fact]
        public void WhenIBuyThenTheValuationIsCorrect()
        {
            SetupAndOrExecute(true);
            var startingNumberOfShares = 10;
            var accountFundMap = _fakeInvestmentRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            Assert.Equal(_numberOfShares + startingNumberOfShares, accountFundMap.Quantity);
            Assert.Equal(0, accountFundMap.Valuation);
        }

        [Fact]
        public void WhenIBuyAndTheAccountIsAnOEICBothTheSellingAndBuyPriceAreRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, FundClasses.Oeic);
            SetupAndOrExecute(true);

            var investmentId = 1;
            var prices = _fakeInvestmentRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().BuyPrice);
            Assert.Equal(_priceOfOneShare, prices.First().SellPrice);
        }

        [Fact]
        public void WhenIBuyAndTheAccountIsAOEICThenTheAccountIsValuedCorrect()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, FundClasses.Oeic);
            SetupAndOrExecute(true);

            var maps = _fakeInvestmentRepository.GetAccountInvestmentMapsByInvestmentId(fakeInvestmentId)
                .Where(map => map.AccountId == _accountId);

            var evaluation = (maps.Single().Quantity) * _priceOfOneShare;

            var account = _fakeInvestmentRepository.GetAccountByAccountId(_accountId);
            Assert.Equal(evaluation, account.Valuation);
        }

        [Fact]
        public void WhenIBuyAndTheAccountIsUnitTrustThenTheAccountIsValuedCorrect()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var account = _fakeInvestmentRepository.GetAccountByAccountId(_accountId);
            Assert.Equal(0, account.Valuation);
        }

        [Fact]
        public void WhenIBuyAndTheAccountIsAUnitTrustFundOnlyTheBuyIsRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var investmentId = 1;
            var prices = _fakeInvestmentRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().BuyPrice);
            Assert.Equal(null, prices.First().SellPrice);
        }

        [Fact]
        public void WhenIBuyTheBuyTransactionAndTheCashTransactionAreLinked()
        {

            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);
            var cashTransaction = _CashTransactionRepository.GetCashTransactionById(arbitaryId);

            Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
            Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

            var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        }

        [Fact]
        public void WhenISellTheSellTransactionAndCommisionTransactionAreLinked()
        {
            var arbitaryId = 1;
            const int commissionId = 2;

            SetupAndOrExecute(true);

            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);

            var cashTransaction = _CashTransactionRepository.GetCashTransactionById(commissionId);

            Assert.NotEqual(Guid.Empty, fundTransaction.LinkedTransaction);
            Assert.NotEqual(Guid.Empty, cashTransaction.LinkedTransaction);
            Assert.Equal(fundTransaction.LinkedTransaction, cashTransaction.LinkedTransaction);

            var linkedTransactionType = TransactionLink.FundToCash().LinkedTransactionType;
            Assert.Equal(linkedTransactionType, fundTransaction.LinkedTransactionType);
            Assert.Equal(linkedTransactionType, cashTransaction.LinkedTransactionType);
        }
    }
}
