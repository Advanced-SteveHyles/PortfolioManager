using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFundSellProcess : BaseProcess<InvestmentSellRequest>
    {
        private readonly InvestmentSellRequest _request;
        private readonly IAccountHandler _accountHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IInvestmentHandler _investmentHandler;

        public RecordFundSellProcess(
            InvestmentSellRequest request,
            IAccountHandler accountHandler,
            ICashTransactionHandler cashTransactionHandler,
            IAccountInvestmentMapProcessor accountInvestmentMapProcessor,
            IFundTransactionHandler fundTransactionHandler,
            IPriceHistoryHandler priceHistoryHandler, IInvestmentHandler investmentHandler)
            :base(request)
        {
            _request = request;
            _accountHandler = accountHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _fundTransactionHandler = fundTransactionHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentHandler = investmentHandler;            
        }

        protected override void ProcessToRun()
        {
            var transactionLink = TransactionLink.FundToCash();
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_request.InvestmentMapId);
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;

            _cashTransactionHandler.StoreCashTransaction(accountId, _request, transactionLink);
            _fundTransactionHandler.StoreFundTransaction(_request, transactionLink);
            var quantityToRemove = 0 -  _request.Quantity;
            _accountInvestmentMapProcessor.ChangeQuantity(_request.InvestmentMapId, quantityToRemove);

            var investment = _investmentHandler.GetInvestment(investmentId);

            var priceRequest = new PriceHistoryRequest
            {
                InvestmentId = investmentId,
                BuyPrice = (investment.Class == FundClasses.Oeic) ? _request.SellPrice : new decimal?(),
                SellPrice = _request.SellPrice,                
                ValuationDate = _request.SellDate
            };

            _priceHistoryHandler.StorePriceHistory(priceRequest, DateTime.Now);

            var singlePriceRequest = new RevalueSinglePriceRequest()
            {
                InvestmentId = investmentId,
                ValuationDate = _request.SellDate
            };

            var revaluePriceTransaction = new RevalueSinglePriceProcess(singlePriceRequest
                , _priceHistoryHandler, _accountInvestmentMapProcessor, _accountHandler );
            revaluePriceTransaction.Execute();
        
        }

        
        protected override bool Validate(InvestmentSellRequest request) => _request.Validate();        
    }
    
}
