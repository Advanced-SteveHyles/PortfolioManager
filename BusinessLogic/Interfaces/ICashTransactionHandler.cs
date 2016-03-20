using System;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface ICashTransactionHandler
    {
        void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest, TransactionLink transactionLink);
        void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest, TransactionLink transactionLink);
        void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest, TransactionLink transactionLink);
        void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest, TransactionLink transactionLink);
        void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest, TransactionLink transactionLink);
    }
}