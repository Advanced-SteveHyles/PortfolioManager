using System;
using System.Text.RegularExpressions;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class RequestValidator
    {
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

        public static bool Validate(this CashTransferRequest request)
        {
            return request.FromAccount !=0 &&
                request.ToAccount !=0 &&
                request.FromAccount != request.ToAccount &&
                request.Amount > 0 &&
                request.TransactionDate != DateTime.MinValue;
        }
    }
}