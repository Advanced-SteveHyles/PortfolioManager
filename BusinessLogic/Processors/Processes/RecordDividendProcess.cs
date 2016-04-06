using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordDividendProcess : BaseProcess<InvestmentDividendRequest>
    {
        private readonly InvestmentDividendRequest _request;
        private readonly FundTransactionHandler _fundTransactionHandler;
        private readonly CashTransactionHandler _cashTransactionHandler;
        private readonly AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        
        public RecordDividendProcess(InvestmentDividendRequest request, FundTransactionHandler fundTransactionHandler, CashTransactionHandler cashTransactionHandler, AccountInvestmentMapProcessor accountInvestmentMapProcessor)
            :base(request)
        {            
            _request = request;
            _fundTransactionHandler = fundTransactionHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;        
        }

        protected override void ProcessToRun()
        {
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_request.InvestmentMapId);
            var accountId = investmentMapDto.AccountId;
            
            TransactionLink linkedTransaction = TransactionLink.FundToCash();
            _cashTransactionHandler.StoreCashTransaction(accountId, _request, linkedTransaction);
            _fundTransactionHandler.StoreFundTransaction(_request, linkedTransaction);

            ExecuteResult = true;
        }

        protected override bool Validate(InvestmentDividendRequest request) => request.Validate();

        public bool ExecuteResult { get; set; }
    }
}