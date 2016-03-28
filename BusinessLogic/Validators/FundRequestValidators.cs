using System;
using System.Net.NetworkInformation;
using Portfolio.Common.DTO.Requests.Transactions;
using static Portfolio.BackEnd.BusinessLogic.Validators.GlobalValidators;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class FundRequestValidators
    {
        public static bool Validate(this InvestmentCorporateActionRequest request)
        {
            return request.InvestmentMapId != 0 && IsValidDate(request.TransactionDate);
        }

        public static bool Validate(this InvestmentLoyaltyBonusRequest request)
        {
            return request.InvestmentMapId != 0 && IsValidDate(request.TransactionDate);
        }

        public static bool Validate(this InvestmentSellRequest request)
        {
            return request.InvestmentMapId != 0 &&
                   IsValidDate(request.SellDate) &&
                   IsValidDate(request.SettlementDate) &&
                   request.Quantity >= 0;
        }

        public static bool Validate(this InvestmentBuyRequest request)
        {
            return request.InvestmentMapId != 0 &&
                    IsValidDate(request.PurchaseDate) &&
                    IsValidDate(request.SettlementDate) &&
                    request.SettlementDate >= request.PurchaseDate &&                   
                   request.Quantity >= 0;
        }

        public static bool Validate(this InvestmentDividendRequest request)
        {
            return request.InvestmentMapId != 0 &&
                request.Amount > 0 && 
                IsValidDate(request.TransactionDate);
        }

        public static bool Validate(this RevalueSinglePriceRequest request)
        {
            return request.InvestmentId != 0 && IsValidDate(request.ValuationDate);
        }
        
    }
}