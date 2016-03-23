using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordDividendTransaction : ICommandRunner
    {
        private readonly InvestmentDividendRequest _request;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        
        public RecordDividendTransaction(InvestmentDividendRequest request, IFundTransactionHandler fundTransactionHandler, ICashTransactionHandler cashTransactionHandler, IAccountInvestmentMapProcessor accountInvestmentMapProcessor)
        {
            _request = request;
            _fundTransactionHandler = fundTransactionHandler;
            _cashTransactionHandler = cashTransactionHandler;
            _accountInvestmentMapProcessor = accountInvestmentMapProcessor;        
        }

        public void Execute()
        {
            var investmentMapDto = _accountInvestmentMapProcessor.GetAccountInvestmentMap(_request.InvestmentMapId);
            var accountId = investmentMapDto.AccountId;
            
            TransactionLink linkedTransaction = TransactionLink.FundToCash();
            _cashTransactionHandler.StoreCashTransaction(accountId, _request, linkedTransaction);
            _fundTransactionHandler.StoreFundTransaction(_request, linkedTransaction);

            ExecuteResult = true;
        }

        public bool CommandValid => _request.Validate();
        public bool ExecuteResult { get; set; }
    }
}