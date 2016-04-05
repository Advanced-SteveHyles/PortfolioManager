using System.Linq;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class PortfolioValuationProcessor : BaseProcess<PortfolioRevaluationRequest>
    {
        private readonly PortfolioRevaluationRequest _request;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public PortfolioValuationProcessor(PortfolioRevaluationRequest request, IPortfolioRepository portfolioRepository, IAccountRepository accountRepository, IAccountInvestmentMapRepository accountInvestmentRepository,  IInvestmentRepository investmentRepository) : base(request)
        {
            _request = request;
            _portfolioRepository = portfolioRepository;
            _accountRepository = accountRepository;
            _accountInvestmentRepository = accountInvestmentRepository;
            _investmentRepository = investmentRepository;
        }

        protected override void ProcessToRun()
        {
            var allAccountsForPortfolio = _accountRepository.GetAccountsForPortfolio(_request.PortfolioId);
            var propertyAccountValue =  allAccountsForPortfolio.Where(acc => acc.Type == PortfolioAccountTypes.Property).Sum(acc=>acc.Cash);
            var cashAccountValue = allAccountsForPortfolio.Where(acc => acc.Type != PortfolioAccountTypes.Property).Sum(acc => acc.Cash);

            var bondAccountValue = (decimal) 0;
            var equityAccountValue = (decimal)0;

            foreach (var account in allAccountsForPortfolio)
            {
                var accountInvestmentMaps = _accountInvestmentRepository.GetAccountInvestmentMapsByAccountId(account.AccountId);

                foreach (var investmentMap in accountInvestmentMaps)
                {
                    var type = _investmentRepository.GetInvestment(investmentMap.InvestmentId).Type;

                    if (type == FundInvestmentTypes.Bond)
                    {
                        bondAccountValue += investmentMap.Valuation.Value;
                    }
                    else if (type == FundInvestmentTypes.Fund || type == FundInvestmentTypes.Tracker)
                    {
                        equityAccountValue += investmentMap.Valuation.Value;
                    }
                }
            }
            

            var total = propertyAccountValue + cashAccountValue + bondAccountValue + equityAccountValue;

            var entityPortfolioValuation = new PortfolioFactory().CreatePortfolioValuation(_request, propertyAccountValue, cashAccountValue, bondAccountValue, equityAccountValue, total);

            _portfolioRepository.UpdatePortfolioValuation(entityPortfolioValuation);
        }

        protected override bool Validate(PortfolioRevaluationRequest request) => request.Validate();
    }
}