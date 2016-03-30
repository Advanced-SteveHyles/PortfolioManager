using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicTests.FakeRepositories;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.Common.DTO.Requests;
using Xunit;

namespace BusinessLogicTests.Processes.Fund.Evaluations
{
    public class PortfolioValuation
    {
        private FakeRepository _fakeRepository;

        public PortfolioValuation()
        {
            _fakeRepository = new FakeRepository();

            var portfolioId = 1;
            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = portfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest);

            portfolioValuationProcessor.Execute();

        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {

            var portfolioValuation = portfolioValuation;

            Assert.Equal(0, portfolioValuation.PropertyValue);
        }
    }
}
