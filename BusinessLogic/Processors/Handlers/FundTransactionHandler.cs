using System;
using Interfaces;
using PortfolioManager.Constants.TransactionTypes;
using PortfolioManager.DTO.Requests;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.DTO.Transactions;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogic.Processors.Handlers
{
    public class FundTransactionHandler : IFundTransactionHandler
    {
        private IFundTransactionRepository _repository;

        public FundTransactionHandler(IFundTransactionRepository repository)
        {
            _repository = repository;
        }

        public void StoreFundTransaction(InvestmentBuyRequest request)
        {
            int? sellPrice =null;
            var source = string.Empty;
            
            StoreFundTransaction(
                request.InvestmentMapId,
                request.PurchaseDate,
                request.SettlementDate,
                source,
                request.Value,
                request.Quantity,
                sellPrice,
                request.Price,
                request.Charges,
                FundTransactionTypes.Buy);
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
                FundTransactionTypes.CorporateAction);
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