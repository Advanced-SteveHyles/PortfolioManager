using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicTests.FakeRepositories;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;
using Xunit;

namespace BusinessLogicTests.Processes.Fund.Evaluations
{
    public class PortfolioValuation
    {
        private readonly FakePortfolioRepository _fakePortfolioRepository;
        private readonly FakeRepository _fakeRepository;

        private const int PortfolioId = 1;

        public PortfolioValuation()
        {
            _fakePortfolioRepository = new FakePortfolioRepository();
            _fakeRepository = new FakeRepository();
            
        }

        private void RevaluePortfolio()
        {
            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = PortfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest,
                _fakePortfolioRepository, _fakeRepository);

            portfolioValuationProcessor.Execute();
        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {
            RevaluePortfolio();
            var  portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioId);
            
            Assert.Equal(0, portfolioValuation.PropertyValue);
        }

        [Fact]
        public void WhenAPortfolioHasNoAPropertyAccountsTheValulationIsTheBalanceOfThePropertyAccount()
        {
            var transactionValue = (decimal)50;
            ApplyCashDeposit(transactionValue);

            RevaluePortfolio();
            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioId);

            Assert.Equal(transactionValue , portfolioValuation.PropertyValue);
        }

        private void ApplyCashDeposit(decimal transactionValue)
        {
            var accountId = FakeData.PropertyAccountId;            
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
