using System;
using System.CodeDom;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordCorporateActionTransaction: ICommandRunner
    {
        private readonly InvestmentCorporateActionRequest _request;
        private readonly IFundTransactionHandler _fundTransactionHandler;
        private readonly ICashTransactionHandler _cashTransactionHandler;
        private readonly IAccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly IInvestmentHandler _investmentHandler;

        public RecordCorporateActionTransaction(InvestmentCorporateActionRequest request, IFundTransactionHandler fundTransactionHandler, ICashTransactionHandler cashTransactionHandler, IAccountInvestmentMapProcessor accountInvestmentMapProcessor, IInvestmentHandler investmentHandler)
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
            var investmentId = investmentMapDto.InvestmentId;
            var accountId = investmentMapDto.AccountId;

            var investment = _investmentHandler.GetInvestment(investmentId);

            _request.ReturnCashToAccount = investment.IncomeType == FundIncomeTypes.Income;

            switch (investment.IncomeType)
            {
                case FundIncomeTypes.Income:
                    _cashTransactionHandler.StoreCashTransaction(accountId, _request);
                    break;
                case FundIncomeTypes.Accumulation:
                    break;
                default:
                    throw new NotSupportedException("Invalid Income Type Supplied");
            }

            _fundTransactionHandler.StoreFundTransaction(_request);

            ExecuteResult = true;
        }

        public bool CommandValid => _request.Validate();
        public bool ExecuteResult { get; set; }
    }
}

