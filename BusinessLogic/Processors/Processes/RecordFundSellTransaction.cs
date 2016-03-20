using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
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
            var transactionLink = TransactionLink.FundToCash();
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_fundSellRequest.InvestmentMapId);
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;

            _cashTransactionHandler.StoreCashTransaction(accountId, _fundSellRequest, transactionLink);
            _fundTransactionHandler.StoreFundTransaction(_fundSellRequest, transactionLink);
            var quantityToRemove = 0 -  _fundSellRequest.Quantity;
            _accountInvestmentMapProcessor.ChangeQuantity(_fundSellRequest.InvestmentMapId, quantityToRemove);

            var investment = _investmentHandler.GetInvestment(investmentId);

            var priceRequest = new PriceHistoryRequest
            {
                InvestmentId = investmentId,
                BuyPrice = (investment.Class == FundClasses.Oeic) ? _fundSellRequest.SellPrice : new decimal?(),
                SellPrice = _fundSellRequest.SellPrice,                
                ValuationDate = _fundSellRequest.SellDate
            };

            _priceHistoryHandler.StorePriceHistory(priceRequest, DateTime.Now);

            var revaluePriceTransaction = new RevalueSinglePriceCommand(
                investmentId,
                _fundSellRequest.SellDate, _priceHistoryHandler, _accountInvestmentMapProcessor, _accountHandler );
            revaluePriceTransaction.Execute();

            ExecuteResult = true;
        }

        public bool CommandValid => _fundSellRequest.Validate();
            
        public bool ExecuteResult { get; private set; }
    }
    
}
