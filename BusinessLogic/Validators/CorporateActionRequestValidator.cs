using System;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
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