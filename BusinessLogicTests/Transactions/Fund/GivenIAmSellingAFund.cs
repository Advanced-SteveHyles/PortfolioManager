using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class GivenIAmSellingAFund
    {
        private decimal _numberOfShares;
        private decimal _priceOfOneShare;
        private decimal _commission;
        private decimal _valueOfTransaction;
        private DateTime _transactionDate;
        private readonly FakeRepository _fakeRepository = new FakeRepository();
        private RecordFundSellTransaction _sellTransaction;
        private int _accountId;

        private IAccountHandler _accountHandler;
        private ICashTransactionHandler _cashCashTransactionHandler;
        private IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private IFundTransactionHandler _fundTransactionHandler;
        private IInvestmentHandler _investmentHandler;

        private DateTime _settlementDate;
        private IPriceHistoryHandler _priceHistoryHandler;
        private int _existingInvestmentMapId = 1;
        private decimal _depositValueWithCommisionPaid;

        public GivenIAmSellingAFund()
        {
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

            _accountHandler = new AccountHandler(_fakeRepository);
            _cashCashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            _priceHistoryHandler = new PriceHistoryHandler(_fakeRepository);
            _investmentHandler = new InvestmentHandler(_fakeRepository);

            _sellTransaction = new RecordFundSellTransaction(request, _accountHandler,
                        _cashCashTransactionHandler, _accountInvestmentMapProcessor,
                        _fundTransactionHandler, _priceHistoryHandler,
                        _investmentHandler);

            if (execute) _sellTransaction.Execute();
        }

        [Fact]
        public void WhenISellTransactionIsValid()
        {
            SetupAndOrExecute(false);
            Assert.True(_sellTransaction.CommandValid);
        }

        [Fact]
        public void WhenISellThenTheAccountValueIsIncreased()
        {
            SetupAndOrExecute(true);

            var account = _fakeRepository.GetAccount(_accountId);
            Assert.Equal(_depositValueWithCommisionPaid, account.Cash);
        }

        [Fact]
        public void WhenISellThenTheAccountHasARecordOfTheDeposit()
        {            
            SetupAndOrExecute(true);

            var cashTransactionId = 1;
            var transaction = _fakeRepository.GetCashTransaction(cashTransactionId);
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
            var transaction = _fakeRepository.GetCashTransaction(cashTransactionId);
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

            var accountFundMap = _fakeRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            var startingNumberOfShares = 10;
            Assert.Equal(startingNumberOfShares - _numberOfShares, accountFundMap.Quantity);
        }

        [Fact]
        public void WhenISellThenTheFundTransactionIsCorrect()
        {
            SetupAndOrExecute(true);

            var arbitaryId = 1;
            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);
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
            _sellTransaction.Execute();

            var arbitaryId = 1;
            var fundTransaction = _fakeRepository.GetFundTransaction(arbitaryId);
            var cashTransaction = _fakeRepository.GetCashTransaction(arbitaryId);

            Assert.Equal(fundTransaction.TransactionValue, cashTransaction.TransactionValue);
        }

        [Fact]
        public void WhenISellThenTheValuationIsCorrect()
        {
            SetupAndOrExecute(true);
            var startingNumberOfShares = 10;
            var accountFundMap = _fakeRepository.GetAccountInvestmentMap(_existingInvestmentMapId);
            Assert.Equal(startingNumberOfShares - _numberOfShares, accountFundMap.Quantity);
            Assert.Equal(0, accountFundMap.Valuation);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAnOEICBothTheSellingAndBuyPriceAreRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeRepository.SetInvestmentClass(fakeInvestmentId, FundClasses.Oeic);
            SetupAndOrExecute(true);

            var investmentId = 1;
            var prices = _fakeRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().BuyPrice);
            Assert.Equal(_priceOfOneShare, prices.First().SellPrice);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAOEICThenTheAccountIsValuedCorrect()
        {
            var fakeInvestmentId = 1;
            _fakeRepository.SetInvestmentClass(fakeInvestmentId, FundClasses.Oeic);
            SetupAndOrExecute(true);

            var maps = _fakeRepository.GetAccountInvestmentMapsByInvestmentId(fakeInvestmentId)
                .Where(map => map.AccountId == _accountId);

            var evaluation = (maps.Single().Quantity) * _priceOfOneShare;

            var account = _fakeRepository.GetAccount(_accountId);
            Assert.Equal(evaluation, account.Valuation);
        }

        [Fact]
        public void WhenISellAndTheAccountIsUnitTrustThenTheAccountIsValuedCorrect()
        {
            var fakeInvestmentId = 1;
            _fakeRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var account = _fakeRepository.GetAccount(_accountId);
            Assert.Equal(0, account.Valuation);
        }

        [Fact]
        public void WhenISellAndTheAccountIsAUnitTrustFundOnlyTheSellPriceIsRecorded()
        {
            var fakeInvestmentId = 1;
            _fakeRepository.SetInvestmentClass(fakeInvestmentId, "UnitTrust");
            SetupAndOrExecute(true);

            var investmentId = 1;
            var prices = _fakeRepository.GetInvestmentBuyPrices(investmentId);

            Assert.Equal(_priceOfOneShare, prices.First().SellPrice);
            Assert.Equal(null, prices.First().BuyPrice);
        }

    }
}
