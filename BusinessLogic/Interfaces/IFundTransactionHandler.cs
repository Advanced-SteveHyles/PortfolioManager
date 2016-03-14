using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface IFundTransactionHandler
    {
        void StoreFundTransaction(InvestmentBuyRequest request);
        void StoreFundTransaction(InvestmentCorporateActionRequest request);
        void StoreFundTransaction(InvestmentSellRequest request);
    }
}