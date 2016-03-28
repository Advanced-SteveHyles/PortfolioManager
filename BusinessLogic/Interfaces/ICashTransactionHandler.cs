using System;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface ICashTransactionHandler
    {
        void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest);
        void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest);
        
        void StoreCashTransaction(CashTransferRequest request, TransactionLink linkedTransaction, string source);

        void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest, TransactionLink transactionLink);
        void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest, TransactionLink transactionLink);
        
        void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest, TransactionLink transactionLink);
        void StoreCashTransaction(int accountId, InvestmentLoyaltyBonusRequest request, TransactionLink linkedTransaction, string source);
        void StoreCashTransaction(int accountId, InvestmentDividendRequest request, TransactionLink linkedTransaction);
    }
}