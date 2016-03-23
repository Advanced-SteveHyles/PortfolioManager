using System;
using Interfaces;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RevalueSinglePriceProcess: IProcess
    {
        private readonly IAccountInvestmentMapProcessor _investmentMapProcessor;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IAccountHandler _accountHandler;
        private readonly int _investmentId;
        private readonly DateTime _valuationDate;


        public RevalueSinglePriceProcess(int investmentId, DateTime valuationDate, IPriceHistoryHandler priceHistoryHandler, IAccountInvestmentMapProcessor investmentMapProcessor, IAccountHandler accountHandler)
        {
            _investmentId = investmentId;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentMapProcessor = investmentMapProcessor;
            _accountHandler = accountHandler;
            _valuationDate = valuationDate;            
        }

        public void Execute()
        {
            var currentSellPrice = _priceHistoryHandler.GetInvestmentSellPrice(_investmentId, _valuationDate);
            var accountsMappedToInvestment = _investmentMapProcessor.GetMapsByInvestmentId(_investmentId);

            foreach (var map in accountsMappedToInvestment)
            {
                var currentValuation = map.Valuation??0;
                RemovePreviousValuationFromAccount(map.AccountId, currentValuation);

                var newValuation = _investmentMapProcessor.RevalueMap(map.AccountInvestmentMapId, currentSellPrice);

                AddNewValuationToAccount(map.AccountId, newValuation);
            }

            ExecuteResult = true;
        }

        private void AddNewValuationToAccount(int accountId, decimal valuation)
        {
            _accountHandler.IncreaseValuation(accountId, valuation);
        }

        private void RemovePreviousValuationFromAccount(int accountId, decimal valuation)
        {
            _accountHandler.DecreaseValuation(accountId, valuation);
        }

        public bool ProcessValid { get; }
        public bool ExecuteResult { get; private set; }
    }
}
