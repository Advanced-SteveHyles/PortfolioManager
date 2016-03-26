using System;
using System.Collections.Generic;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualPriceHistoryController
    {
        private readonly IAccountInvestmentMapRepository _accountInvestmentMapRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public VirtualPriceHistoryController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(connection);
            _investmentRepository = new InvestmentRepository(connection);
            _priceHistoryRepository = new PriceHistoryRepository(connection);
            _accountRepository = new AccountRepository(connection);

        }

        public void UpdateAllPrices()
        {
            var request = new RevalueAllPricesRequest()
            {
                EvaluationDate = DateTime.Now
            };

            var revalueAllPricesCommand = new RevalueAllPricesProcess(
                request,
                new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                new InvestmentHandler(_investmentRepository),
                new PriceHistoryHandler(_priceHistoryRepository),
                new AccountHandler(_accountRepository)
                );

            revalueAllPricesCommand.Execute();
        }

        public void SavePriceHistories(IEnumerable<PriceHistoryRequest> requests)
        {
            var priceHistoryHandler = new PriceHistoryHandler(_priceHistoryRepository);

            foreach (var request in requests)
            {
                if (UpdateRequired(request)) continue;

                var revalueAllPricesCommand = new RecordPriceHistoryProcess(
                    request,
                    priceHistoryHandler
                    );

                revalueAllPricesCommand.Execute();
            }

            UpdateAllPrices();
        }

        private static bool UpdateRequired(PriceHistoryRequest request)
        {
            return (request.BuyPrice.HasValue && request.BuyPrice > 0) 
                    ||
                   (request.SellPrice.HasValue && request.SellPrice > 0);
        }
    }
}