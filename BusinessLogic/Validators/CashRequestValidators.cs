using System;
using System.Linq;
using System.Text.RegularExpressions;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;
using static Portfolio.BackEnd.BusinessLogic.Validators.GlobalValidators;

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


        public static bool Validate(this WithdrawalTransactionRequest request) => request.AccountId > 0
                                                                                  && request.Value > 0
                                                                                  && IsValidDate(request.TransactionDate)
                                                                                  && !string.IsNullOrWhiteSpace(request.Source)
                                                                                  && CashWithdrawalTransactionTypes.WithdrawalTypes.Contains(request.TransactionType);

        public static bool Validate(this DepositTransactionRequest request) => request.AccountId > 0
                                                                               && request.Value > 0
                                                                               && IsValidDate(request.TransactionDate)
                                                                               && !string.IsNullOrWhiteSpace(request.Source)
                                                                               && CashDepositTransactionTypes.DepositTypes.Contains(request.TransactionType);

        public static bool Validate(this CheckpointRequest request)
        {
            return request.AccountId > 0
                   && IsValidDate(request.FromDate)
                   && IsValidDate(request.ToDate)
                   && request.ItemsToCheckpoint.TrueForAll(item => item.AccountId == request.AccountId)
                   && request.ItemsToCheckpoint.TrueForAll(item => !item.CheckpointId.HasValue);
        }
    }
}