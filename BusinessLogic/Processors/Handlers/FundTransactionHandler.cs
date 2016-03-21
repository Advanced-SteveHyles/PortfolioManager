using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
{
    public class FundTransactionHandler : IFundTransactionHandler
    {
        private readonly IFundTransactionRepository _repository;
        

        public FundTransactionHandler(IFundTransactionRepository repository)
        {
            _repository = repository;            
        }

        public void StoreFundTransaction(InvestmentBuyRequest request, TransactionLink transactionLink)
        {            
        int? sellPrice = null;
            var source = string.Empty;

            StoreFundTransaction(
                request.InvestmentMapId,
                request.PurchaseDate,
                request.SettlementDate,
                source,
                request.Value,
                request.Quantity,
                sellPrice,
                request.BuyPrice,
                request.Charges,
                FundTransactionTypes.Buy,
                transactionLink);
        }

        public void StoreFundTransaction(InvestmentSellRequest request, TransactionLink transactionLink)
        {
            int? buyPrice = null;
            var source = string.Empty;

            StoreFundTransaction(
                request.InvestmentMapId,
                request.SellDate,
                request.SettlementDate,
                source,
                request.Value,
                request.Quantity,
                request.SellPrice,
                buyPrice,
                request.Charges,
                FundTransactionTypes.Sell,
                transactionLink);
        }

        public void StoreFundTransaction(InvestmentLoyaltyBonusRequest request, TransactionLink linkedTransaction)
        {
            decimal? buyPrice = null;
            decimal? sellPrice = null;
            const int quantity = 0;
            const decimal charges = 0;
            const decimal transactionValue = 0;
            const string source = "";

            StoreFundTransaction(
                request.InvestmentMapId,
                request.TransactionDate,
                request.TransactionDate,
                source,
                transactionValue,
                quantity,
                sellPrice,
                buyPrice,
                charges,
                FundTransactionTypes.LoyaltyBonus,
                linkedTransaction);
        }

        public void StoreFundTransaction(InvestmentCorporateActionRequest request, TransactionLink transactionLink)
        {
            int? sellPrice = null;
            int? buyPrice = null;
            var source = string.Empty;
            var quantity = 0;

            var charges = 0;

            StoreFundTransaction(
                request.InvestmentMapId,
                request.TransactionDate,
                request.TransactionDate,
                source,
                request.Amount,
                quantity,
                sellPrice,
                buyPrice,
                charges,
                request.ReturnCashToAccount ? FundTransactionTypes.ReturnOfCapital : FundTransactionTypes.CorporateAction,
                transactionLink
            );
        }

        private void StoreFundTransaction(int investmentMapId, DateTime transactionDate, DateTime settlementDate, string source, decimal transactionValue, decimal quantity, decimal? sellPrice, decimal? buyPrice, decimal charges, string transactionType, TransactionLink transactionLink)
        {
            var fundTransaction = new CreateFundTransactionRequest()
            {

                InvestmentMapId = investmentMapId,
                TransactionType = transactionType,
                TransactionDate = transactionDate,
                SettlementDate = settlementDate,
                Source = source,
                Quantity = quantity,
                SellPrice = sellPrice,
                BuyPrice = buyPrice,
                Charges = charges,
                TransactionValue = transactionValue,
                LinkedTransaction = transactionLink?.LinkedTransaction,
                LinkedTransactionType = transactionLink?.LinkedTransactionType,
            };

            if (fundTransaction.SettlementDate < fundTransaction.TransactionDate)
            {
                fundTransaction.SettlementDate = fundTransaction.TransactionDate;
            }

            _repository.InsertFundTransaction(fundTransaction);
        }
    }
}