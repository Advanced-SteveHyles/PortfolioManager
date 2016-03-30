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
        const int PortfolioId = 1;

        public PortfolioValuation()
        {
            _fakeRepository = new FakeRepository();

            
            PortfolioRevaluationRequest portfolioRevaluationRequest = new PortfolioRevaluationRequest()
            {
                PortfolioId = PortfolioId
            };

            var portfolioValuationProcessor = new PortfolioValuationProcessor(portfolioRevaluationRequest);

            portfolioValuationProcessor.Execute();

        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {
          var  portfolioValuation = _fakeRepository.GetPortfolioValuation(PortfolioId);
            
            Assert.Equal(0, portfolioValuation.PropertyValue);
        }
    }
}
