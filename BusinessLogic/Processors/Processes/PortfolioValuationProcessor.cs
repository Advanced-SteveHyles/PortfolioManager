using System.Linq;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class PortfolioValuationProcessor : BaseProcess<PortfolioRevaluationRequest>
    {
        private readonly PortfolioRevaluationRequest _request;
        private readonly IPortfolioRepository _portfolioRepository;
        private IAccountRepository _accountRepository;

        public PortfolioValuationProcessor(PortfolioRevaluationRequest request, IPortfolioRepository portfolioRepository, IAccountRepository accountRepository) : base(request)
        {
            _request = request;
            _portfolioRepository = portfolioRepository;
            _accountRepository = accountRepository;
        }

        protected override void ProcessToRun()
        {
            var x = _accountRepository.GetAccountsForPortfolio(_request.PortfolioId);
            var propertyAccountValue =  x.Where(acc => acc.Type == PortfolioAccountTypes.Property).Sum(acc=>acc.Cash);
            
            var entityPortfolioValuation = new PortfolioFactory().CreatePortfolioValuation(_request, propertyAccountValue);

            _portfolioRepository.UpdatePortfolioValuation(entityPortfolioValuation);
        }

        protected override bool Validate(PortfolioRevaluationRequest request) => request.Validate();
    }
}