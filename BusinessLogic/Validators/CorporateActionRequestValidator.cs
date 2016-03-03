using System;
using PortfolioManager.DTO.Requests.Transactions;

namespace BusinessLogic.Validators
{
    public static class CorporateActionRequestValidator
    {
        public static bool Validate(this InvestmentCorporateActionRequest request)
        {
            return request.InvestmentMapId != 0 &&
                   request.TransactionDate != DateTime.MinValue;
        }
    }
}