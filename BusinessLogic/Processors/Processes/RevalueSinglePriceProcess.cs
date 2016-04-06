using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RevalueSinglePriceProcess: BaseProcess<RevalueSinglePriceRequest>
    {
        private readonly AccountInvestmentMapProcessor _investmentMapProcessor;
        private readonly PriceHistoryHandler _priceHistoryHandler;
        private readonly AccountHandler _accountHandler;
        private readonly RevalueSinglePriceRequest _request;
        
        public RevalueSinglePriceProcess(RevalueSinglePriceRequest request,PriceHistoryHandler priceHistoryHandler, AccountInvestmentMapProcessor investmentMapProcessor, AccountHandler accountHandler) 
            : base(request)
        {
            _request = request;            
            _priceHistoryHandler = priceHistoryHandler;
            _investmentMapProcessor = investmentMapProcessor;
            _accountHandler = accountHandler;            
        }

        protected override void ProcessToRun()
        {
            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(_request.InvestmentId, _request.ValuationDate);
            var accountsMappedToInvestment = _investmentMapProcessor.GetMapsByInvestmentId(_request.InvestmentId);

            foreach (var map in accountsMappedToInvestment)
            {
                var currentValuation = map.Valuation??0;
                RemovePreviousValuationFromAccount(map.AccountId, currentValuation);

                var newValuation = _investmentMapProcessor.RevalueMap(map.AccountInvestmentMapId, currentSellPrice);

                AddNewValuationToAccount(map.AccountId, newValuation);
            }        
        }

        protected override bool Validate(RevalueSinglePriceRequest request) => _request.Validate();
        
        private void AddNewValuationToAccount(int accountId, decimal valuation)
        {
            _accountHandler.IncreaseValuation(accountId, valuation);
        }

        private void RemovePreviousValuationFromAccount(int accountId, decimal valuation)
        {
            _accountHandler.DecreaseValuation(accountId, valuation);
        }
        
    }
}
