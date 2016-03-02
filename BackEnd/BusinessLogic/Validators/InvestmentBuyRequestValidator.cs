using System;
using PortfolioManager.DTO.Transactions;

namespace BusinessLogic.Validators
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