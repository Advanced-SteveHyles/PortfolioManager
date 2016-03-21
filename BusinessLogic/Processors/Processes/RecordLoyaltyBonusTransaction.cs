﻿using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordLoyaltyBonusTransaction
    {
        private readonly InvestmentLoyaltyBonusRequest _request;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IInvestmentHandler _investmentHandler;

        public RecordLoyaltyBonusTransaction(InvestmentLoyaltyBonusRequest request, IFundTransactionHandler fundTransactionHandler, ICashTransactionHandler cashTransactionHandler, IAccountInvestmentMapProcessor accountInvestmentMapProcessor, IInvestmentHandler investmentHandler)
        {
            _request = request;
            _fundTransactionHandler = fundTransactionHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _investmentHandler = investmentHandler;
        }

        public void Execute()
        {
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_request.InvestmentMapId);
            var accountId = investmentMapDto.AccountId;
            var linkedTransaction = TransactionLink.FundToCash();
            var investment = _investmentHandler.GetInvestment(investmentMapDto.InvestmentId);

            _cashTransactionHandler.StoreCashTransaction(accountId, _request, linkedTransaction);
            _fundTransactionHandler.StoreFundTransaction(_request, linkedTransaction);

            ExecuteResult = true;
        }

        public bool CommandValid => _request.Validate();
        public bool ExecuteResult { get; set; }

    }
}