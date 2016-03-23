using System;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class FundRequestValidators
    {
        public static bool Validate(this InvestmentCorporateActionRequest request)
        {
            return request.InvestmentMapId != 0 &&
                   request.TransactionDate != DateTime.MinValue;
        }
    
        public static bool Validate(this InvestmentLoyaltyBonusRequest request)
        {
            return request.InvestmentMapId != 0 &&
                   request.TransactionDate != DateTime.MinValue;
        }

        public static bool Validate(this InvestmentSellRequest request)
        {
            return request.InvestmentMapId != 0 &&
                   request.SellDate != DateTime.MinValue &&
                   request.SettlementDate != DateTime.MinValue &&
                   request.Quantity >= 0;
        }

        public static bool Validate(this InvestmentBuyRequest request)
        {
            if (request.SettlementDate < request.PurchaseDate)
            {
                request.SettlementDate = request.PurchaseDate;
            }

            return request.InvestmentMapId != 0 &&
                   request.PurchaseDate != DateTime.MinValue &&
                   request.Quantity >= 0;
        }

        public static bool Validate(this InvestmentDividendRequest request)
        {
            return request.InvestmentMapId != 0 &&
                request.Amount > 0 &&
                request.TransactionDate != DateTime.MinValue;
        }

    }
}