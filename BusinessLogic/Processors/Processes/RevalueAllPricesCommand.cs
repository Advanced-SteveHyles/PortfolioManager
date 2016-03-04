using System;
using System.Linq;
using Interfaces;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RevalueAllPricesCommand : ICommandRunner
    {
        private readonly IAccountInvestmentMapProcessor _investmentMapProcessor;
        private readonly IInvestmentHandler _investmentHandler;
        private readonly IPriceHistoryHandler _priceHistoryHandler;
        private readonly IAccountHandler _accountHandler;
        private readonly DateTime _evaluationDate;
        
        public RevalueAllPricesCommand(DateTime evaluationDate, IAccountInvestmentMapProcessor investmentMapProcessor, IInvestmentHandler investmentHandler, IPriceHistoryHandler priceHistoryHandler, IAccountHandler accountHandler)
        {
            _investmentMapProcessor = investmentMapProcessor;
            _investmentHandler = investmentHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _accountHandler = accountHandler;
            _evaluationDate = evaluationDate;
        }

        public void Execute()
        {            
            RevalueAllMaps();
            UpdateAllAccounts();           
            ExecuteResult = true;
        }

        private void UpdateAllAccounts()
        {
            foreach (var account in _accountHandler.GetAccounts().ToList())
            {
                var investmentMaps = _investmentMapProcessor.GetMapsByAccountId(account.AccountId);
                var valuation = investmentMaps.Sum(inv => inv.Valuation) ?? 0;
                
                _accountHandler.SetValuation(account.AccountId, valuation);
            }
        }

        private void RevalueAllMaps()
        {
            foreach (var investment in _investmentHandler.GetInvestments().ToList())
            {
                RevalueMapsForInvestment(investment.InvestmentId);
            }
        }

        private void RevalueMapsForInvestment(int investmentId)
        {
            var investmentSellPriceAtDate = _priceHistoryHandler.GetInvestmentSellPrice(investmentId, _evaluationDate);
            var investmentMaps = _investmentMapProcessor.GetMapsByInvestmentId(investmentId);
            foreach (var map in investmentMaps)
            {
                _investmentMapProcessor.RevalueMap(map.AccountInvestmentMapId, investmentSellPriceAtDate);
            }
        }

        public bool CommandValid => true;
        public bool ExecuteResult { get; private set; }
    }
}