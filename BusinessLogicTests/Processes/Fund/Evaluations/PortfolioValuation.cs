using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicTests.FakeRepositories;
using Xunit;

namespace BusinessLogicTests.Processes.Fund.Evaluations
{
    public class PortfolioValuation
    {
        private FakeRepository _fakeRepository;

        public PortfolioValuation()
        {
            _fakeRepository = new FakeRepository();
        }

        [Fact]
        public void WhenAPortfolioHasNoAccountsTheValulationIsZero()
        {
            var portfolioValuation = portfolioValuation;


            Assert(0, portfolioValuation.PropertyValue);
        }
    }
}
