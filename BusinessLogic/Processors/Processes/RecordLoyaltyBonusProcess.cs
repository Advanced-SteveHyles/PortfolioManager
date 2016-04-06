using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.BusinessLogic.Validators;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Processes
{
    public class RecordLoyaltyBonusProcess : BaseProcess<InvestmentLoyaltyBonusRequest>
    {
        private readonly InvestmentLoyaltyBonusRequest _request;
        private readonly FundTransactionHandler _fundTransactionHandler;
        private readonly CashTransactionHandler _cashTransactionHandler;
        private readonly AccountInvestmentMapProcessor _accountInvestmentMapProcessor;
        private readonly InvestmentHandler _investmentHandler;

        public RecordLoyaltyBonusProcess(InvestmentLoyaltyBonusRequest request, FundTransactionHandler fundTransactionHandler, CashTransactionHandler cashTransactionHandler, AccountInvestmentMapProcessor accountInvestmentMapProcessor, InvestmentHandler investmentHandler)
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
            var accountId = investmentMapDto.AccountId;
            var linkedTransaction = TransactionLink.FundToCash();
            var investment = _investmentHandler.GetInvestment(investmentMapDto.InvestmentId);

            _cashTransactionHandler.StoreCashTransaction(accountId, _request, linkedTransaction, investment.Name);
            _fundTransactionHandler.StoreFundTransaction(_request, linkedTransaction);            
        }
        
        protected override bool Validate(InvestmentLoyaltyBonusRequest request) => _request.Validate();
        
    }
}