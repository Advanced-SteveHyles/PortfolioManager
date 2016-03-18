using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Processors.Processes;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualInvestmentTransactions
    {
        private readonly IAccountInvestmentMapRepository _accountInvestmentMapRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        private readonly IFundTransactionRepository _fundTransactionRepository;
        private readonly ICashTransactionRepository _cashTransactionRepository;

        public VirtualInvestmentTransactions()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _accountInvestmentMapRepository = new AccountInvestmentMapRepository(connection);
            _fundTransactionRepository = new FundTransactionRepository(connection);
            _priceHistoryRepository = new PriceHistoryRepository(connection);
            _cashTransactionRepository = new CashTransactionRepository(connection);
            _accountRepository = new AccountRepository(connection);
            _investmentRepository = new InvestmentRepository(connection);
        }


        public void Buy(InvestmentBuyRequest purchaseRequest)
        {
                if (purchaseRequest == null)
                {
                    throw new InvalidOperationException();
                }

                var createFundBuyTransaction = new RecordFundBuyTransaction
                    (purchaseRequest,
                        new AccountHandler(_accountRepository),
                        new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                        new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                        new FundTransactionHandler(_fundTransactionRepository),
                        new PriceHistoryHandler(_priceHistoryRepository),
                        new InvestmentHandler(_investmentRepository)
                    );

                var status = CommandExecutor.ExecuteCommand
                    (
                        createFundBuyTransaction
                    );


            if (status == false)
            {
                throw new InvalidOperationException("Transaction Failed" );
            }
        }
    }
}
