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

            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = PortfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest, _fakePortfolioRepository);

            portfolioValuationProcessor.Execute();

        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {
          var  portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioId);
            
            Assert.Equal(0, portfolioValuation.PropertyValue);
        }

        [Fact]
        public void WhenAPortfolioHasNoAPropertyAccountsTheValulationIsTheBalanceOfThePropertyAccount()
        {
            var accountId = FakeData.PropertyAccountId;
            var transactionValue = 50;
            var depositTransactionRequest = new DepositTransactionRequest
            {
                AccountId = accountId,
                Value = transactionValue,
                Source = "Property Equity",
                TransactionDate = DateTime.Today,
                TransactionType = CashDepositTransactionTypes.Deposit
            };


            var _cashTransactionHandler = new CashTransactionHandler(_fakeRepository, _fakeRepository);

            var depositTransaction = new RecordDepositProcess(depositTransactionRequest, _cashTransactionHandler, null);
            depositTransaction.Execute();

            var portfolioValuation = _fakePortfolioRepository.GetPortfolioValuation(PortfolioId);

            Assert.Equal(transactionValue , portfolioValuation.PropertyValue);
        }

    }
}
