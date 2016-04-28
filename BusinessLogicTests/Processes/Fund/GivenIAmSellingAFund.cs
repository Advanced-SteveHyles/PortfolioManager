using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BusinessLogicTests.FakeRepositories;
using BusinessLogicTests.Fakes;
using BusinessLogicTests.Fakes.DataFakes;
using Interfaces;
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
    public class GivenIAmSellingAFund
    {
        private decimal _numberOfShares;
        private decimal _priceOfOneShare;
        private decimal _commission;
        private decimal _valueOfTransaction;
        private DateTime _transactionDate;
        private readonly FakeInvestmentRepository _fakeInvestmentRepository = new FakeInvestmentRepository(new FakeDataGeneric());
        private RecordFundSellProcess _sellProcess;
        private int _accountId;

        private AccountHandler _accountHandler;
        private CashTransactionHandler _cashCashTransactionHandler;
        private AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private FundTransactionHandler _fundTransactionHandler;
        private InvestmentHandler _investmentHandler;

        private DateTime _settlementDate;
        private PriceHistoryHandler _priceHistoryHandler;
        private int _existingInvestmentMapId = 1;
        private decimal _depositValueWithCommisionPaid;
        private ICashTransactionRepository _CashTransactionRepository;

        public GivenIAmSellingAFund()
        {
            _CashTransactionRepository = new FakeCashTransactionRepository(new FakeDataGeneric());
            _depositValueWithCommisionPaid = _valueOfTransaction - _commission;
        }

        private void SetupAndOrExecute(bool execute)
        {

            _numberOfShares = 10;
            _priceOfOneShare = 1;
            _commission = 2;
            _valueOfTransaction = (_numberOfShares * _priceOfOneShare);
            _depositValueWithCommisionPaid = _valueOfTransaction - _commission;
            _transactionDate = DateTime.Now;
            _settlementDate = DateTime.Today.AddDays(14);
            _accountId = 1;

            var request = new InvestmentSellRequest
            {
                InvestmentMapId = _existingInvestmentMapId,
                Quantity = _numberOfShares,
                SellPrice = _priceOfOneShare,
                SellDate = _transactionDate,
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

            _sellProcess = new RecordFundSellProcess(request, _accountHandler,
                        _cashCashTransactionHandler, _accountInvestmentMapProcessor,
                        _fundTransactionHandler, _priceHistoryHandler,
                        _investmentHandler);

            if (execute) _sellProcess.Execute();
        }

        [Fact]
        public void WhenISellThenTheAccountValueIsIncreased()
        {
            SetupAndOrExecute(true);

            var account = _fakeInvestmentRepository.GetAccountByAccountId(_accountId);
            Assert.Equal(_depositValueWithCommisionPaid, account.Cash);
        }

        [Fact]
        public void WhenISellThenTheAccountHasARecordOfTheDeposit()
        {            
            SetupAndOrExecute(true);

            var cashTransactionId = 1;
            var transaction = _CashTransactionRepository.GetCashTransactionById(cashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_depositValueWithCommisionPaid, transaction.TransactionValue -_commission);
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.FundSale, transaction.TransactionType);
        }

        public void WhenISellThenTheAccountHasARecordOfTheCommision()
        {
            SetupAndOrExecute(true);

            var cashTransactionId = 2;
            var transaction = _CashTransactionRepository.GetCashTransactionById(cashTransactionId);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(_transactionDate, transaction.TransactionDate);
            Assert.Equal(_commission, transaction.TransactionValue);
            Assert.Equal(string.Empty, transaction.Source);
            Assert.Equal(false, transaction.IsTaxRefund);
            Assert.Equal(CashTransactionTypes.Commission, transaction.TransactionType);
        }

        [Fact]
        public void WhenIBuyThenTheShareCountIsDecreased()
        {
            SetupAndOrExecute(true);

            var accountFundMap = _fakeInvestmentRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            var startingNumberOfShares = 10;
            Assert.Equal(startingNumberOfShares - _numberOfShares, accountFundMap.Quantity);
        }

        [Fact]
        public void WhenISellThenTheFundTransactionIsCorrect()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);
            Assert.Equal(_priceOfOneShare, fundTransaction.SellPrice);
            Assert.Equal(FundTransactionTypes.Sell, fundTransaction.TransactionType);
            Assert.Equal(_transactionDate, fundTransaction.TransactionDate);
            Assert.Equal(_settlementDate, fundTransaction.SettlementDate);
            Assert.Equal(string.Empty, fundTransaction.Source);
            Assert.Equal(_numberOfShares, fundTransaction.Quantity);
            Assert.Equal(_priceOfOneShare, fundTransaction.SellPrice);
            Assert.Equal(null, fundTransaction.BuyPrice);
            Assert.Equal(_commission, fundTransaction.Charges);
            
            Assert.Equal(_valueOfTransaction, fundTransaction.TransactionValue);
        }


        [Fact]
        public void WhenISellThenTheFundTransactionAndTheCashTransactionValueAreIdenticalButOpposite()
        {
            SetupAndOrExecute(true);
            _sellProcess.Execute();

            var arbitaryId = 1;
            var fundTransaction = _fakeInvestmentRepository.GetFundTransaction(arbitaryId);
            var cashTransaction = _CashTransactionRepository.GetCashTransactionById(arbitaryId);

            Assert.Equal(fundTransaction.TransactionValue, cashTransaction.TransactionValue);
        }

        [Fact]
        public void WhenISellThenTheValuationIsCorrect()
        {
            SetupAndOrExecute(true);
            var startingNumberOfShares = 10;
            var accountFundMap = _fakeInvestmentRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            Assert.Equal(startingNumberOfShares - _numberOfShares, accountFundMap.Quantity);
            Assert.Equal(0, accountFundMap.Valuation);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAnOEICBothTheSellingAndBuyPriceAreRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, FundClasses.Oeic);
            SetupAndOrExecute(true);

            var investmentId = FakeDataGeneric.FakeInvestmentId;
            var prices = _fakeInvestmentRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().BuyPrice);
            Assert.Equal(_priceOfOneShare, prices.First().SellPrice);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAOEICThenTheAccountIsValuedCorrect()
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
        public void WhenISellAndTheAccountIsUnitTrustThenTheAccountIsValuedCorrect()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var account = _fakeInvestmentRepository.GetAccountByAccountId(_accountId);
            Assert.Equal(0, account.Valuation);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAUnitTrustFundOnlyTheSellPriceIsRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeInvestmentRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var investmentId = 1;
            var prices = _fakeInvestmentRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().SellPrice);
            Assert.Equal(null, prices.First().BuyPrice);
        }


        [Fact]
        public void WhenISellTheSellTransactionAndCashTransactionAreLinked()
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
