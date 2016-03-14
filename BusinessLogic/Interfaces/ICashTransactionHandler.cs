using Portfolio.Common.DTO.Requests.Transactions;

namespace Interfaces
{
    public interface ICashTransactionHandler
    {
        void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest);
        void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest);
        void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest);
        void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest);
        void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest);
    }
}