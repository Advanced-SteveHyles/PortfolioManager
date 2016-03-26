using System;
using System.Text.RegularExpressions;
using Portfolio.Common.DTO.Requests.Transactions;
using static Portfolio.BackEnd.BusinessLogic.Validators.Validators;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class CashRequestValidators
    {
        public static bool Validate(this CashTransferRequest request)
        {
            return request.FromAccount != 0 &&
                request.ToAccount != 0 &&
                request.FromAccount != request.ToAccount &&
                request.Amount > 0 && IsValidDate(request.TransactionDate);
        }


        public static bool Validate(this WithdrawalTransactionRequest request)
        {
            return request.AccountId > 0
                                    && request.Value > 0
                                    && IsValidDate(request.TransactionDate)
                                    && !string.IsNullOrWhiteSpace(request.Source);
        }

        public static bool Validate(this FeeTransactionRequest request)
        {
            return request.AccountId > 0
                                    && request.Value > 0
                                    && IsValidDate(request.TransactionDate);
        }


        public static bool Validate(this DepositTransactionRequest request)
        {
            return request.AccountId > 0
                   && request.Value > 0
                   && IsValidDate(request.TransactionDate)
                   && !string.IsNullOrWhiteSpace(request.Source);
        }
    }
}