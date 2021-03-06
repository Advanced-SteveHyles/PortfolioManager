using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFundBuyProcess : BaseProcess<InvestmentBuyRequest>
    {
        private readonly InvestmentBuyRequest _fundBuyRequest;
        private readonly AccountHandler _accountHandler;
        private readonly CashTransactionHandler _cashTransactionHandler;
        private readonly AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly FundTransactionHandler _fundTransactionHandler;
        private readonly PriceHistoryHandler _priceHistoryHandler;
        private readonly InvestmentHandler _investmentHandler;
        

        public RecordFundBuyProcess(
            InvestmentBuyRequest request,
            AccountHandler accountHandler,
            CashTransactionHandler cashTransactionHandler,
            AccountInvestmentMapProcessor accountInvestmentMapProcessor,
            FundTransactionHandler fundTransactionHandler,
            PriceHistoryHandler priceHistoryHandler, 
            InvestmentHandler investmentHandler)
            :base(request)
        {
            _fundBuyRequest = request;
            _accountHandler = accountHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _fundTransactionHandler = fundTransactionHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentHandler = investmentHandler;            
        }

        protected override void ProcessToRun()
        {
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_fundBuyRequest.InvestmentMapId);
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;
            var  transactionLink = TransactionLink.FundToCash();

            _cashTransactionHandler.StoreCashTransaction(accountId, _fundBuyRequest, transactionLink);
            _fundTransactionHandler.StoreFundTransaction(_fundBuyRequest, transactionLink);
            _accountInvestmentMapProcessor.ChangeQuantity(_fundBuyRequest.InvestmentMapId, _fundBuyRequest.Quantity);

            var investment = _investmentHandler.GetInvestment(investmentId);

            var priceRequest = new PriceHistoryRequest
            {
                InvestmentId = investmentId,
                BuyPrice = _fundBuyRequest.BuyPrice,
                SellPrice = (investment.Class == FundClasses.Oeic) ? _fundBuyRequest.BuyPrice : new decimal?(),
                ValuationDate = _fundBuyRequest.PurchaseDate
            };

            _priceHistoryHandler.StorePriceHistory(priceRequest, DateTime.Now);

            var singlePriceRequest = new RevalueSinglePriceRequest()
            {
                InvestmentId = investmentId,
                ValuationDate = _fundBuyRequest.PurchaseDate
            };

            var revaluePriceTransaction = new RevalueSinglePriceProcess(singlePriceRequest,
                _priceHistoryHandler, _accountInvestmentMapProcessor, _accountHandler);
            revaluePriceTransaction.Execute();        
        }
        
        protected override bool Validate(InvestmentBuyRequest request) => _fundBuyRequest.Validate();        
    }
}