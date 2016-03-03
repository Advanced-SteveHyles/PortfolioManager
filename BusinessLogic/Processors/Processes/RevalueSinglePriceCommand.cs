using System;
using Interfaces;

namespace BusinessLogic.Processors.Processes
{
    public class RevalueSinglePriceCommand: ICommandRunner
    {
        private readonly IAccountInvestmentMapProcessor _investmentMapProcessor;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IAccountHandlers _accountHandlers;
        private readonly int _investmentId;
        private readonly DateTime _valuationDate;


        public RevalueSinglePriceCommand(int investmentId, DateTime valuationDate, IPriceHistoryHandler priceHistoryHandler, IAccountInvestmentMapProcessor investmentMapProcessor, IAccountHandlers accountHandlers)
        {
            _investmentId = investmentId;
            _priceHistoryHandler = priceHistoryHandler;
            _investmentMapProcessor = investmentMapProcessor;
            _accountHandlers = accountHandlers;
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
            _accountHandlers.IncreaseValuation(accountId, valuation);
        }

        private void RemovePreviousValuationFromAccount(int accountId, decimal valuation)
        {
            _accountHandlers.DecreaseValuation(accountId, valuation);
        }

        public bool CommandValid { get; }
        public bool ExecuteResult { get; private set; }
    }
}
