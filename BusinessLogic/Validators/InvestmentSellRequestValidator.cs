using System;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class InvestmentSellRequestValidator
    {
        public static bool Validate(this InvestmentSellRequest request)
        {            
            return request.InvestmentMapId != 0 &&
                   request.SellDate != DateTime.MinValue &&
                   request.SettlementDate != DateTime.MinValue &&
                   request.Quantity >=0;
        }
    }
}