using System;
using Interfaces;
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

        public void StoreFundTransaction(InvestmentBuyRequest request)
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
                FundTransactionTypes.Buy);
        }

        public void StoreFundTransaction(InvestmentSellRequest request)
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
                FundTransactionTypes.Sell);
        }

        public void StoreFundTransaction(InvestmentCorporateActionRequest request)
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
                request.ReturnCashToAccount ? FundTransactionTypes.ReturnOfCapital : FundTransactionTypes.CorporateAction
            );
        }

        private void StoreFundTransaction(
            int investmentMapId,
            DateTime transactionDate,
            DateTime settlementDate,
            string source,
            decimal transactionValue,
            decimal quantity,
            decimal? sellPrice,
            decimal? buyPrice,
            decimal charges,
            string transactionType)
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
            };

            if (fundTransaction.SettlementDate < fundTransaction.TransactionDate)
            {
                fundTransaction.SettlementDate = fundTransaction.TransactionDate;
            }

            _repository.InsertFundTransaction(fundTransaction);
        }
    }
}