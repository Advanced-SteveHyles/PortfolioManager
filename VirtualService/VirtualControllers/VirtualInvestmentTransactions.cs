﻿using System;
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
            var createFundBuyProcess = new RecordFundBuyProcess
                (purchaseRequest,
                    new AccountHandler(_accountRepository),
                    new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                    new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                    new FundTransactionHandler(_fundTransactionRepository),
                    new PriceHistoryHandler(_priceHistoryRepository),
                    new InvestmentHandler(_investmentRepository)
                );

            createFundBuyProcess.Execute();

            if (createFundBuyProcess.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }

        public void Sell(InvestmentSellRequest saleRequest)
        {
            var createFundBuyProcess = new RecordFundSellProcess
                (saleRequest,
                    new AccountHandler(_accountRepository),
                    new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                    new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                    new FundTransactionHandler(_fundTransactionRepository),
                    new PriceHistoryHandler(_priceHistoryRepository),
                    new InvestmentHandler(_investmentRepository)
                );

            createFundBuyProcess.Execute();

            if (createFundBuyProcess.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }


        public void LoyaltyBonus(InvestmentLoyaltyBonusRequest loyaltyBonusRequest)
        {           
            var loyaltyBonusProcess = new RecordLoyaltyBonusProcess
                (loyaltyBonusRequest,
                new FundTransactionHandler(_fundTransactionRepository),
                    new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                    new AccountInvestmentMapProcessor(_accountInvestmentMapRepository),
                    new InvestmentHandler(_investmentRepository)
            );


            loyaltyBonusProcess.Execute();

            if (loyaltyBonusProcess.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }

        public void Divdend(InvestmentDividendRequest investmentDividendRequest)
        {
            
            var recordDividendProcess = new RecordDividendProcess
                (investmentDividendRequest,
                new FundTransactionHandler(_fundTransactionRepository),
                    new CashTransactionHandler(_cashTransactionRepository, _accountRepository),
                    new AccountInvestmentMapProcessor(_accountInvestmentMapRepository)                    
            );


            recordDividendProcess.Execute();

            if (recordDividendProcess.ExecuteResult == false)
            {
                throw new InvalidOperationException("Transaction Failed");
            }
        }
    }
}
