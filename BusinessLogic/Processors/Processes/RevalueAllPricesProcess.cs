using System;
using System.Linq;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RevalueAllPricesProcess : BaseProcess<RevalueAllPricesRequest>
    {
        private readonly AccountInvestmentMapProcessor _investmentMapProcessor;
        private readonly InvestmentHandler _investmentHandler;
        private readonly PriceHistoryHandler _priceHistoryHandler;
        private readonly AccountHandler _accountHandler;
        private readonly DateTime _evaluationDate;
        
        public RevalueAllPricesProcess(RevalueAllPricesRequest request, AccountInvestmentMapProcessor investmentMapProcessor, InvestmentHandler investmentHandler, PriceHistoryHandler priceHistoryHandler, AccountHandler accountHandler)
            :base(request)
        {
            _investmentMapProcessor = investmentMapProcessor;
            _investmentHandler = investmentHandler;
            _priceHistoryHandler = priceHistoryHandler;
            _accountHandler = accountHandler;
            _evaluationDate = request.EvaluationDate;
        }

        protected override void ProcessToRun()
        {            
            RevalueAllMaps();
            UpdateAllAccounts();                    
        }
                
        protected override bool Validate(RevalueAllPricesRequest request) => request.EvaluationDate != DateTime.MinValue;
    
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
    }
}