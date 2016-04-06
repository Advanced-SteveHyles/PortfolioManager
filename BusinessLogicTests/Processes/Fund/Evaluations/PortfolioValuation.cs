using System;
using BusinessLogicTests.FakeRepositories;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;
using static BusinessLogicTests.FakeRepositories.FakeDataForPortfolioValuation;

namespace BusinessLogicTests.Processes.Fund.Evaluations
{
    public class PortfolioValuation
    {
        private readonly FakePortfolioRepository _fakePortfolioRepository;
        private readonly FakeRepository _fakeRepository;

        private const decimal Commission = 2;

        public PortfolioValuation()
        {
            _fakePortfolioRepository = new FakePortfolioRepository();
            _fakeRepository = new FakeRepository(new FakeDataForPortfolioValuation());            
        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {
            RevaluePortfolio(PortfolioWithNoAccounts);
            var  portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithNoAccounts);
            
            Assert.Equal(0, portfolioValuation.PropertyValue);
        }

        [Fact]
        public void WhenAPortfolioHasAPropertyAccountTheValulationIsTheBalanceOfThePropertyAccount()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyOnlyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyOnlyAccount);

            Assert.Equal(transactionValue , portfolioValuation.PropertyValue);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAPropertyAccountTheRatioOfPropertyIsOneHundredPercent()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyOnlyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyOnlyAccount);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.PropertyRatio);
        }

        [Fact]
        public void WhenAPortfolioHasANonPropertyAccountTheValulationIsTheBalanceOfTheCashOnAccounts()
        {
            var transactionValue = (decimal)22.50;
            ApplyCashDeposit(transactionValue, SavingsAccountForPortfolioWithOnlySavingsAccount);

            RevaluePortfolio(PortfolioWithOnlySavingsAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithOnlySavingsAccount);

            Assert.Equal(transactionValue, portfolioValuation.CashValue);
        }
        
        [Fact]
        public void WhenAPortfolioANonPropertyAccountTheRatioOfCashIsOneHundredPercent()
        {
            var transactionValue = (decimal)25;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyOnlyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyOnlyAccount);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.PropertyRatio);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToABondTheValulationIsTheBalanceOfBonds()
        {         
            var transactionValue = ApplyFundPurchase(BondAccountInvestmentMap);
            
            RevaluePortfolio(PortfolioWithAccountLinkedToBond);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAccountLinkedToBond);
            
            Assert.Equal(transactionValue, portfolioValuation.BondValue);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToABondTheRatioOfPropertyIsOneHundredPercent()
        {
            var transactionValue = ApplyFundPurchase(BondAccountInvestmentMap);
            ApplyCashDeposit(transactionValue + Commission, BondAccountForPortfolioWithOnlyBondsAccounts);
            RevaluePortfolio(PortfolioWithAccountLinkedToBond);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAccountLinkedToBond);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.BondRatio);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToFundTheValulationIsTheBalanceOfFund()
        {
            var transactionValue = ApplyFundPurchase(EquityAccountInvestmentMap);
            
            RevaluePortfolio(PortfolioWithAccountLinkedToEquity);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAccountLinkedToEquity);

            Assert.Equal(transactionValue, portfolioValuation.EquityValue);
        }


        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToAFundTheRatioOfPropertyIsOneHundredPercent()
        {
            var transactionValue = ApplyFundPurchase(EquityAccountInvestmentMap);
            ApplyCashDeposit(transactionValue + Commission, EquityAccountForPortfolioWithOnlyEquityAccounts);

            RevaluePortfolio(PortfolioWithAccountLinkedToEquity);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAccountLinkedToEquity);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.EquityRatio);
        }

        [Fact]
        public void WhenAPortfolioContainsAllAccountTypesTheRatiosAreCorrect()
        {
            //Property 10
            //Cash, 10, 10, 10
            //Fund 0, 10

            const decimal transactionValue = (decimal)10;

            ApplyCashDeposit(transactionValue, SavingsAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, CashIsaAccountForPortfolioWithAllAccountTypes);

            ApplyCashDeposit(12, StockIsaAccountForPortfolioWithAllAccountTypes);
            ApplyFundPurchase(StockIsaAccountMap);

            ApplyCashDeposit(transactionValue, PensionAccountForPortfolioWithAllAccountTypes);

            RevaluePortfolio(PortfolioWithAllAccountTypes);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAllAccountTypes);
            
            const decimal expectedPropertyRatio = (decimal).2;
            const decimal expectedCashRatio = (decimal) .6; 

            const decimal expectedBondRatio = (decimal)0;
            const decimal expectedEquityRatio = (decimal).2;
            
            Assert.Equal(expectedPropertyRatio, portfolioValuation.PropertyRatio);
            Assert.Equal(expectedCashRatio, portfolioValuation.CashRatio);
            Assert.Equal(expectedBondRatio, portfolioValuation.BondRatio);
            Assert.Equal(expectedEquityRatio, portfolioValuation.EquityRatio);
        }


        [Fact]
        public void WhenAPortfolioContainsAllAccountTypesAndNoSharesTheRatiosAreCorrect()
        {
            const decimal transactionValue = (decimal)50;

            ApplyCashDeposit(transactionValue, SavingsAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, CashIsaAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, StockIsaAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, PensionAccountForPortfolioWithAllAccountTypes);

            RevaluePortfolio(PortfolioWithAllAccountTypes);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAllAccountTypes);

            const decimal expectedPropertyRatio = (decimal).2; // 0.2252
            const decimal expectedCashRatio = (decimal).8; //0.7748;

            const decimal expectedBondRatio = (decimal)0;
            const decimal expectedEquityRatio = (decimal)0;

            Assert.Equal(expectedPropertyRatio, Math.Round(portfolioValuation.PropertyRatio, 4));
            Assert.Equal(expectedCashRatio, Math.Round(portfolioValuation.CashRatio, 4));
            Assert.Equal(expectedBondRatio, Math.Round(portfolioValuation.BondRatio, 4));
            Assert.Equal(expectedEquityRatio, Math.Round(portfolioValuation.EquityRatio, 4));
        }


        private decimal ApplyFundPurchase(int existingInvestmentMapId)
        {
            var  _numberOfShares = 10;
            var _priceOfOneShare = 1;            
            var _valueOfTransaction = (_numberOfShares * _priceOfOneShare);
            var _transactionDate = DateTime.Now;
            var _settlementDate = DateTime.Today.AddDays(14);

            var request = new InvestmentBuyRequest
            {
                InvestmentMapId = existingInvestmentMapId,
                Quantity = _numberOfShares,
                BuyPrice = _priceOfOneShare,
                PurchaseDate = _transactionDate,
                SettlementDate = _settlementDate,
                UpdatePriceHistory = false,
                Value = _valueOfTransaction,
                Charges = Commission
            };

            var     _accountHandler = new AccountHandler(_fakeRepository);
            var _cashCashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);
            var _accountInvestmentMapProcessor = new AccountInvestmentMapProcessor(_fakeRepository);
            var _fundTransactionHandler = new FundTransactionHandler(_fakeRepository);
            var _priceHistoryHandler = new PriceHistoryHandler(_fakeRepository);
            var _investmentHandler = new InvestmentHandler(_fakeRepository);

            var _buyProcess = new RecordFundBuyProcess(request, _accountHandler,
                _cashCashTransactionHandler, _accountInvestmentMapProcessor,
                _fundTransactionHandler, _priceHistoryHandler,
                _investmentHandler);

            _buyProcess.Execute();
            
            RevalueInvestments(_accountInvestmentMapProcessor, _investmentHandler, _priceHistoryHandler, _accountHandler);

            return _valueOfTransaction;
        }

        private static void RevalueInvestments(AccountInvestmentMapProcessor _accountInvestmentMapProcessor,
            InvestmentHandler _investmentHandler, PriceHistoryHandler _priceHistoryHandler, AccountHandler _accountHandler)
        {
            var revalueRequest = new RevalueAllPricesRequest()
            {
                EvaluationDate = DateTime.Today.AddDays(1)
            };


            var _valuatioinProcess = new RevalueAllPricesProcess(
                revalueRequest, _accountInvestmentMapProcessor, _investmentHandler,
                _priceHistoryHandler, _accountHandler
                );


            _valuatioinProcess.Execute();
        }


        private void RevaluePortfolio(int portfolioId)
        {
            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = portfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest,
                _fakePortfolioRepository, _fakeRepository, _fakeRepository, _fakeRepository);

            portfolioValuationProcessor.Execute();
        }

        private void ApplyCashDeposit(decimal transactionValue, int accountId)
        {            
            var depositTransactionRequest = new DepositTransactionRequest
            {
                AccountId = accountId,
                Value = transactionValue,
                Source = "Property Equity",
                TransactionDate = DateTime.Today,
                TransactionType = CashDepositTransactionTypes.Deposit
            };

            var cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);

            var depositTransaction = new RecordDepositProcess(depositTransactionRequest, cashTransactionHandler, null);
            depositTransaction.Execute();
        }
    }
}
