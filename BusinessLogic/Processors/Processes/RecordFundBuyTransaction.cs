using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFundBuyTransaction : ICommandRunner
    {
        private readonly InvestmentBuyRequest _fundBuyRequest;
        private readonly IAccountHandler _accountHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IInvestmentHandler _investmentHandler;
        

        public RecordFundBuyTransaction(
            InvestmentBuyRequest fundBuyRequest,
            IAccountHandler accountHandler,
            ICashTransactionHandler cashTransactionHandler,
            IAccountInvestmentMapProcessor accountInvestmentMapProcessor,
            IFundTransactionHandler fundTransactionHandler,
            IPriceHistoryHandler priceHistoryHandler, IInvestmentHandler investmentHandler)
        {
            _fundBuyRequest = fundBuyRequest;
            _accountHandler = accountHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _fundTransactionHandler = fundTransactionHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentHandler = investmentHandler;            
        }

        public void Execute()
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

            var revaluePriceTransaction = new RevalueSinglePriceCommand(
                investmentId,
                _fundBuyRequest.PurchaseDate, _priceHistoryHandler, _accountInvestmentMapProcessor, _accountHandler);
            revaluePriceTransaction.Execute();

            ExecuteResult = true;
        }

        public bool CommandValid => _fundBuyRequest.Validate();

        public bool ExecuteResult { get; private set; }
    }
}