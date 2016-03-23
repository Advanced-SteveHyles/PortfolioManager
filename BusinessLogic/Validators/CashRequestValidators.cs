using System;
using System.Text.RegularExpressions;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class CashRequestValidators
    {
        

        public static bool Validate(this CashTransferRequest request)
        {
            return request.FromAccount != 0 &&
                request.ToAccount != 0 &&
                request.FromAccount != request.ToAccount &&
                request.Amount > 0 &&
                request.TransactionDate != DateTime.MinValue;
        }

    
        public static bool Validate(this WithdrawalTransactionRequest withdrawalTransactionRequest)
        {
            return withdrawalTransactionRequest.AccountId > 0
                                    && withdrawalTransactionRequest.Value > 0
                                    && withdrawalTransactionRequest.TransactionDate != DateTime.MinValue
                                    && !string.IsNullOrWhiteSpace(withdrawalTransactionRequest.Source);
        }

        public static bool Validate(this FeeTransactionRequest feeTransactionRequest)
        {
            return feeTransactionRequest.AccountId > 0
                                    && feeTransactionRequest.Value > 0
                                    && feeTransactionRequest.TransactionDate != DateTime.MinValue;
        }


        public static bool Validate(this DepositTransactionRequest depositTransactionRequest)
        {
            return depositTransactionRequest.AccountId > 0
                   && depositTransactionRequest.Value > 0
                   && depositTransactionRequest.TransactionDate != DateTime.MinValue
                   && !string.IsNullOrWhiteSpace(depositTransactionRequest.Source)
                ;
        }
    }
}