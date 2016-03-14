using System;
using Interfaces;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.Constants.TransactionTypes;
using Portfolio.Common.DTO.Requests;
using Portfolio.Common.DTO.Requests.Transactions;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
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

            StoreCommision(accountId, investmentBuyRequest.PurchaseDate, source, investmentBuyRequest.Charges);
        }

        private void StoreCommision(int accountId, DateTime purchaseDate, string source, decimal commision)
        {
            if (commision == 0)
                return;

            StoreCashTransaction(
                          accountId,
                          purchaseDate,
                          source,
                          commision,
                          false,
                          CashTransactionTypes.Commission,
                          increaseAccountBalance: false
                          );
        }


        public void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest)
        {
            var source = string.Empty;
            StoreCashTransaction(
                          accountId,
                          investmentSellRequest.SellDate,
                          source,
                          investmentSellRequest.Value,
                          false,
                          CashTransactionTypes.FundSale,
                          increaseAccountBalance: true
                          );

            StoreCommision(accountId, investmentSellRequest.SellDate, source, investmentSellRequest.Charges);
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
                          CashTransactionTypes.CashRefund,
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