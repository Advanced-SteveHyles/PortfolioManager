using System;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualPriceHistoryController
    {
        private readonly IAccountInvestmentMapRepository _accountInvestmentMapRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly AccountRepository _accountRepository;
        private readonly PriceHistoryRepository _priceHistoryRepository;

        public VirtualPriceHistoryController(string connection)
        {
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(connection);
            _investmentRepository = new InvestmentRepository(connection);
            _priceHistoryRepository = new PriceHistoryRepository(connection);
            _accountRepository = new AccountRepository(connection);

        }

        public void UpdateAllPrices()
        {
            var revalueAllPricesCommand = new RevalueAllPricesCommand(
                DateTime.Now, 
                new AccountInvestmentMapProcessor(_accountInvestmentMapRepository), 
                new InvestmentHandler(_investmentRepository), 
                new PriceHistoryHandler(_priceHistoryRepository), 
                new AccountHandler(_accountRepository)
                );

            revalueAllPricesCommand.Execute();
        }
    }
}