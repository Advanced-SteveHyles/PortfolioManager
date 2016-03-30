using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class PortfolioValuationProcessor : BaseProcess<PortfolioRevaluationRequest>
    {
        public PortfolioValuationProcessor(PortfolioRevaluationRequest request) : base(request)
        {
        }

        protected override void ProcessToRun()
        {
            throw new System.NotImplementedException();
        }

        protected override bool Validate(PortfolioRevaluationRequest request) => request.Validate();
    }
}