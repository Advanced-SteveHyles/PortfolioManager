using System;
using BusinessLogicTests.FakeRepositories;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;
using static BusinessLogicTests.FakeRepositories.FakeData;

namespace BusinessLogicTests.Processes.Fund.Evaluations
{
    public class PortfolioValuation
    {
        private readonly FakePortfolioRepository _fakePortfolioRepository;
        private readonly FakeRepository _fakeRepository;



        public PortfolioValuation()
        {
            _fakePortfolioRepository = new FakePortfolioRepository();
            _fakeRepository = new FakeRepository();            
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

            RevaluePortfolio(PortfolioWithPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyAccount);

            Assert.Equal(transactionValue , portfolioValuation.PropertyValue);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAPropertyAccountTheRatioOfPropertyIsOneHundredPercent()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyAccount);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.PropertyRatio);
        }

        [Fact]
        public void WhenAPortfolioHasANonPropertyAccountTheValulationIsTheBalanceOfTheCashOnAccounts()
        {
            var transactionValue = (decimal)22.50;
            ApplyCashDeposit(transactionValue, SavingsAccountForPortfolioWithOnlySavingsAccount);

            RevaluePortfolio(PortfolioWithoutPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithoutPropertyAccount);

            Assert.Equal(transactionValue, portfolioValuation.CashValue);
        }


        [Fact]
        public void WhenAPortfolioANonPropertyAccountTheRatioOfCashIsOneHundredPercent()
        {
            var transactionValue = (decimal)25;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyAccount);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.PropertyRatio);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToABondTheValulationIsTheBalanceOfBonds()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyAccount);

            var expectedRatio = 1;
            Assert.Equal(transactionValue, portfolioValuation.BondValue);
        }

        [Fact]
        public void WhenAPortfolioOnlyHasAnAccountLinkedToABondTheRatioOfPropertyIsOneHundredPercent()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithOnlyPropertyAccount);

            RevaluePortfolio(PortfolioWithPropertyAccount);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithPropertyAccount);

            var expectedRatio = 1;
            Assert.Equal(expectedRatio, portfolioValuation.BondRatio);
        }

        [Fact]
        public void WhenAPortfolioContainsAllAccountTypesTheRatiosAreCorrect()
        {
            const decimal transactionValue = (decimal)25;
            ApplyCashDeposit(transactionValue, SavingsAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(transactionValue, PropertyAccountForPortfolioWithAllAccountTypes);
            ApplyCashDeposit(50, CashIsaAccountForPortfolioWithAllAccountTypes);

            RevaluePortfolio(PortfolioWithAllAccountTypes);
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioWithAllAccountTypes);

            const decimal expectedPropertyRatio = (decimal) .25;
            const decimal expectedCashRatio = (decimal).75;

            Assert.Equal(expectedPropertyRatio, portfolioValuation.PropertyRatio);
            Assert.Equal(expectedCashRatio, portfolioValuation.CashRatio);            
        }


        private void RevaluePortfolio(int portfolioId)
        {
            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = portfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest,
                _fakePortfolioRepository, _fakeRepository);

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
