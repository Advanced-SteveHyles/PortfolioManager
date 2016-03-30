using System.Runtime.Remoting.Messaging;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class PortfolioValidators
    {
        public static bool Validate(this PortfolioRevaluationRequest request) => request.PortfolioId > 0;
    }
}