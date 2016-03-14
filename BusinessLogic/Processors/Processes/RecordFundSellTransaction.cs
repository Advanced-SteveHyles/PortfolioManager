using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordFundSellTransaction : ICommandRunner
    {
        private readonly InvestmentSellRequest _fundSellRequest;
        private readonly IAccountHandler _accountHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IInvestmentHandler _investmentHandler;

        public RecordFundSellTransaction(
            InvestmentSellRequest fundSellRequest,
            IAccountHandler accountHandler,
            ICashTransactionHandler cashTransactionHandler,
            IAccountInvestmentMapProcessor accountInvestmentMapProcessor,
            IFundTransactionHandler fundTransactionHandler,
            IPriceHistoryHandler priceHistoryHandler, IInvestmentHandler investmentHandler)
        {
            _fundSellRequest = fundSellRequest;
            _accountHandler = accountHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _fundTransactionHandler = fundTransactionHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentHandler = investmentHandler;
        }

        public void Execute()
        {
            
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_fundSellRequest.InvestmentMapId);
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;

            _cashTransactionHandler.StoreCashTransaction(accountId, _fundSellRequest);
            _fundTransactionHandler.StoreFundTransaction(_fundSellRequest);            
            _accountInvestmentMapProcessor.ChangeQuantity(_fundSellRequest.InvestmentMapId, _fundSellRequest.Quantity);

            var investment = _investmentHandler.GetInvestment(investmentId);

            var priceRequest = new PriceHistoryRequest
            {
                InvestmentId = investmentId,
                BuyPrice = _fundSellRequest.Price,
                SellPrice = (investment.Class == FundClasses.Oeic) ? _fundSellRequest.Price : new decimal?(),
                ValuationDate = _fundSellRequest.PurchaseDate
            };

            _priceHistoryHandler.StorePriceHistory(priceRequest, DateTime.Now);

            var revaluePriceTransaction = new RevalueSinglePriceCommand(
                investmentId,
                _fundSellRequest.PurchaseDate, _priceHistoryHandler, _accountInvestmentMapProcessor, _accountHandler );
            revaluePriceTransaction.Execute();

            ExecuteResult = true;
        }

        public bool CommandValid => _fundSellRequest.Validate();
            
        public bool ExecuteResult { get; private set; }
    }
    
}
