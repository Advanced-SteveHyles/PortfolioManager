﻿using System;
using System.CodeDom;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCorporateActionProcess: BaseProcess<InvestmentCorporateActionRequest>
    {
        private readonly InvestmentCorporateActionRequest _request;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IInvestmentHandler _investmentHandler;

        public RecordCorporateActionProcess(InvestmentCorporateActionRequest request, IFundTransactionHandler fundTransactionHandler, ICashTransactionHandler cashTransactionHandler, IAccountInvestmentMapProcessor accountInvestmentMapProcessor, IInvestmentHandler investmentHandler)
            :base(request)
        {            
            _request = request;
            _fundTransactionHandler = fundTransactionHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;
            _investmentHandler = investmentHandler;            
        }

        protected override void ProcessToRun()
        {
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_request.InvestmentMapId);
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;

            var investment = _investmentHandler.GetInvestment(investmentId);

            _request.ReturnCashToAccount = investment.IncomeType == FundIncomeTypes.Income;

            TransactionLink linkedTransaction = null;
            switch (investment.IncomeType)
            {
                case FundIncomeTypes.Income:
                    linkedTransaction = TransactionLink.FundToCash();
                    _cashTransactionHandler.StoreCashTransaction(accountId, _request, linkedTransaction);
                    break;
                case FundIncomeTypes.Accumulation:
                    break;
                default:
                    throw new NotSupportedException("Invalid Income Type Supplied");
            }

            _fundTransactionHandler.StoreFundTransaction(_request, linkedTransaction);            
        }
        
        protected override bool Validate(InvestmentCorporateActionRequest request) => _request.Validate();        
    }
}

