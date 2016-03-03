using System;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class InvestmentBuyRequestValidator
    {
        public static bool Validate(this InvestmentBuyRequest request)
        {
            if (request.SettlementDate < request.PurchaseDate)
            {
                request.SettlementDate = request.PurchaseDate;
            }

            return request.InvestmentMapId != 0 &&
                   request.PurchaseDate != DateTime.MinValue;
        }
    }
}