using System;
using Interfaces;
using PortfolioManager.Constants.TransactionTypes;
using PortfolioManager.DTO.Requests;
using PortfolioManager.DTO.Requests.Transactions;
using PortfolioManager.DTO.Transactions;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogic.Processors.Handlers
{
    public class CashTransactionHandler : ICashTransactionHandler
    {
        private readonly ICashTransactionRepository _repository;
        private readonly IAccountRepository _accountRepository;

        public CashTransactionHandler(ICashTransactionRepository repository, IAccountRepository accountRepository)
        {
            _repository = repository;
            _accountRepository = accountRepository;
        }

        public void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest)
        {
            StoreCashTransaction(
                depositTransactionRequest.AccountId,
                depositTransactionRequest.TransactionDate,
                depositTransactionRequest.Source,
                depositTransactionRequest.Value,
                depositTransactionRequest.IsTaxRefund,
                 CashTransactionTypes.Deposit,
                 increaseAccountBalance: true
                );            
        }

        public void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest)
        {
            StoreCashTransaction(
                      withdrawalTransactionRequest.AccountId,
                      withdrawalTransactionRequest.TransactionDate,
                      withdrawalTransactionRequest.Source,
                      withdrawalTransactionRequest.Value,
                      false,
                  CashTransactionTypes.Withdrawal,
                  increaseAccountBalance:false
                      );
        }

        public void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest)
        {
            var source = string.Empty;       
            StoreCashTransaction(
                          accountId,
                          investmentBuyRequest.PurchaseDate,
                          source,
                          investmentBuyRequest.Value,
                          false,
                          CashTransactionTypes.FundPurchase,
                          increaseAccountBalance: false
                          );
        }

        public void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest)
        {
            var source = string.Empty;
            StoreCashTransaction(
                          accountId,
                          investmentCorporateActionRequest.TransactionDate,
                          source,
                          investmentCorporateActionRequest.Amount,
                          false,
                          CashTransactionTypes.CorporateAction,
                         increaseAccountBalance: true
                          );
        }

        private void StoreCashTransaction(int accountId, DateTime transactionDate, string source, decimal value, bool isTaxRefund, string transactionType, bool increaseAccountBalance)
        {
            var cashTransaction = new CreateCashTransactionRequest()
            {
                AccountId = accountId,
                TransactionDate = transactionDate,
                TransactionType = transactionType,
                TransactionValue = value,
                Source = source,
                IsTaxRefund = isTaxRefund,
            };

            _repository.InsertCashTransaction(cashTransaction);

            if (increaseAccountBalance == true)
            {
                _accountRepository.IncreaseAccountBalance(accountId, value);
            }
            else
            {
                _accountRepository.DecreaseAccountBalance(accountId, value);
            }

        }
    }

  
}