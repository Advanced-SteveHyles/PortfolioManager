using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface IFundTransactionHandler
    {
        void StoreFundTransaction(InvestmentBuyRequest request, TransactionLink transactionLink);
        void StoreFundTransaction(InvestmentCorporateActionRequest request, TransactionLink transactionLink);
        void StoreFundTransaction(InvestmentSellRequest request, TransactionLink transactionLink);
        void StoreFundTransaction(InvestmentLoyaltyBonusRequest request, TransactionLink linkedTransaction);
    }
}