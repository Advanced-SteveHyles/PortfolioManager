﻿using System.Collections.Generic;

namespace Portfolio.Common.Constants.TransactionTypes
{
    public class CashTransactionTypes
    {
       
        public const string Withdrawal = "Withdrawal";
        public const string FundPurchase = "Fund Purchase";
        public const string FundSale = "Fund Sale";
        public const string CorporateAction = "Corporate Action";
        public const string Commission = "Commission";
        public const string LoyaltyBonus = "Loyalty Bonus";
        public const string CashTransferOut = "Cash Transfer Out";
        public const string CashTransferIn = "Cash Transfer In";
        public const string Dividend = "Dividend";
        public const string Fees = "Fees";
    }

    public static class CashDepositTransactionTypes
    {
        public const string Deposit = "Deposit";
        public const string InterestPaid = "Interest Paid";

        public static List<string> DepositTypes { get; } = new List<string>()
        {
             Deposit,
            InterestPaid
        };        
    }
}
