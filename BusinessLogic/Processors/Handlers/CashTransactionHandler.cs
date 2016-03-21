using System;
using Interfaces;
using Portfolio.BackEnd.BusinessLogic.Linking;
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

        public void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest, TransactionLink transactionLink)
        {
            const bool increaseAccountBalance = true;
            StoreCashTransaction(
                depositTransactionRequest.AccountId,
                depositTransactionRequest.TransactionDate,
                depositTransactionRequest.Source,
                depositTransactionRequest.Value,
                depositTransactionRequest.IsTaxRefund,
                 CashTransactionTypes.Deposit,
                 increaseAccountBalance, transactionLink);
        }

        public void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest, TransactionLink transactionLink)
        {
            const bool increaseAccountBalance = false;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                      withdrawalTransactionRequest.AccountId,
                      withdrawalTransactionRequest.TransactionDate,
                      withdrawalTransactionRequest.Source,
                      withdrawalTransactionRequest.Value,
                      isTaxRefund,
                  CashTransactionTypes.Withdrawal,
                   increaseAccountBalance, transactionLink);
        }

        public void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool increaseAccountBalance = false;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentBuyRequest.PurchaseDate,
                          source,
                          investmentBuyRequest.Value,
                          isTaxRefund,
                          CashTransactionTypes.FundPurchase,
                          increaseAccountBalance, transactionLink);

            StoreCommision(accountId, investmentBuyRequest.PurchaseDate, source, investmentBuyRequest.Charges, transactionLink);
        }

        private void StoreCommision(int accountId, DateTime purchaseDate, string source, decimal commision, TransactionLink transactionLink)
        {
            if (commision == 0)
                return;

            const bool increaseAccountBalance = false;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          purchaseDate,
                          source,
                          commision,
                          isTaxRefund,
                          CashTransactionTypes.Commission,
                          increaseAccountBalance, transactionLink);
        }


        public void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool  increaseAccountBalance = true;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentSellRequest.SellDate,
                          source,
                          investmentSellRequest.Value,
                          isTaxRefund,
                          CashTransactionTypes.FundSale,
                          increaseAccountBalance, transactionLink);

            StoreCommision(accountId, investmentSellRequest.SellDate, source, investmentSellRequest.Charges, transactionLink);
        }


        public void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool increaseAccountBalance = true;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentCorporateActionRequest.TransactionDate,
                          source,
                          investmentCorporateActionRequest.Amount,
                          isTaxRefund,
                          CashTransactionTypes.CorporateAction,
                         increaseAccountBalance, transactionLink);
        }

        public void StoreCashTransaction(int accountId, InvestmentLoyaltyBonusRequest _request, TransactionLink linkedTransaction, string source)
        {
            const bool increaseAccountBalance = true;
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          _request.TransactionDate,
                          source,
                          _request.Amount,
                          isTaxRefund,
                          CashTransactionTypes.LoyaltyBonus,
                         increaseAccountBalance, linkedTransaction);
        }

        private void StoreCashTransaction(int accountId, DateTime transactionDate, string source, decimal value, bool isTaxRefund, string transactionType, bool increaseAccountBalance, TransactionLink transactionLink)
        {
            var cashTransaction = new CreateCashTransactionRequest()
            {
                AccountId = accountId,
                TransactionDate = transactionDate,
                TransactionType = transactionType,
                TransactionValue = value,
                Source = source,
                IsTaxRefund = isTaxRefund,
                LinkedTransaction = transactionLink?.LinkedTransaction,
                LinkedTransactionType = transactionLink?.LinkedTransactionType
            };

            _repository.InsertCashTransaction(cashTransaction);

            if (increaseAccountBalance)
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