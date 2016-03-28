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

        public void StoreCashTransaction(DepositTransactionRequest depositTransactionRequest)
        {
            TransactionLink noTransactionLink = null;
            StoreCashTransaction(
                depositTransactionRequest.AccountId,
                depositTransactionRequest.TransactionDate,
                depositTransactionRequest.Source,
                depositTransactionRequest.Value,
                depositTransactionRequest.IsTaxRefund,
                 depositTransactionRequest.TransactionType,
                 noTransactionLink);
        }

        public void StoreCashTransaction(WithdrawalTransactionRequest withdrawalTransactionRequest)
        {
            const bool isTaxRefund = false;
            TransactionLink noTransactionLink = null;
            var negatedValue = withdrawalTransactionRequest.Value.Negate();
            StoreCashTransaction(
                      withdrawalTransactionRequest.AccountId,
                      withdrawalTransactionRequest.TransactionDate,
                      withdrawalTransactionRequest.Source,
                      negatedValue,
                      isTaxRefund,
                      CashTransactionTypes.Withdrawal,
                      noTransactionLink);
        }

        public void StoreCashTransaction(FeeTransactionRequest feeTransactionRequest)
        {
            const bool isTaxRefund = false;
            TransactionLink noTransactionLink = null;
            const string source = "";
            StoreCashTransaction(
                      feeTransactionRequest.AccountId,
                      feeTransactionRequest.TransactionDate,
                      source,
                      -feeTransactionRequest.Value,
                      isTaxRefund,
                  CashTransactionTypes.Fees, noTransactionLink);
        }


        public void StoreCashTransaction(int accountId, InvestmentBuyRequest investmentBuyRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentBuyRequest.PurchaseDate,
                          source,
                          -investmentBuyRequest.Value,
                          isTaxRefund,
                          CashTransactionTypes.FundPurchase, transactionLink);

            StoreCommision(accountId, investmentBuyRequest.PurchaseDate, source, investmentBuyRequest.Charges.Negate(), transactionLink);
        }


        public void StoreCashTransaction(int accountId, InvestmentSellRequest investmentSellRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentSellRequest.SellDate,
                          source,
                          investmentSellRequest.Value,
                          isTaxRefund,
                          CashTransactionTypes.FundSale, transactionLink);

            StoreCommision(accountId, investmentSellRequest.SellDate, source, investmentSellRequest.Charges.Negate(), transactionLink);
        }


        public void StoreCashTransaction(int accountId, InvestmentCorporateActionRequest investmentCorporateActionRequest, TransactionLink transactionLink)
        {
            const string source = "";
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          investmentCorporateActionRequest.TransactionDate,
                          source,
                          investmentCorporateActionRequest.Amount,
                          isTaxRefund,
                          CashTransactionTypes.CorporateAction, transactionLink);
        }

        public void StoreCashTransaction(int accountId, InvestmentLoyaltyBonusRequest _request, TransactionLink linkedTransaction, string source)
        {
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          accountId,
                          _request.TransactionDate,
                          source,
                          _request.Amount,
                          isTaxRefund,
                          CashTransactionTypes.LoyaltyBonus, linkedTransaction);
        }

        public void StoreCashTransaction(int accountId, InvestmentDividendRequest request, TransactionLink linkedTransaction)
        {
            const bool isTaxRefund = false;
            var source = "";
            StoreCashTransaction(
                          accountId,
                          request.TransactionDate,
                          source,
                          request.Amount,
                          isTaxRefund,
                          CashTransactionTypes.Dividend, linkedTransaction);
        }

        public void StoreCashTransaction(CashTransferRequest request, TransactionLink linkedTransaction, string source)
        {
            const bool isTaxRefund = false;
            StoreCashTransaction(
                          request.FromAccount,
                          request.TransactionDate,
                          source,
                          -request.Amount,
                          isTaxRefund,
                          CashTransactionTypes.CashTransferOut, linkedTransaction);

            StoreCashTransaction(
              request.ToAccount,
              request.TransactionDate,
              source,
              request.Amount,
              isTaxRefund,
              CashTransactionTypes.CashTransferIn, linkedTransaction);
        }

        private void StoreCashTransaction(int accountId, DateTime transactionDate, string source, decimal value, bool isTaxRefund, string transactionType, TransactionLink transactionLink)
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
            _accountRepository.AdjustAccountBalance(accountId, value);
        }

        private void StoreCommision(int accountId, DateTime purchaseDate, string source, decimal commision, TransactionLink transactionLink)
        {
            if (commision == 0)
                return;

            const bool isTaxRefund = false;
            StoreCashTransaction(
                accountId,
                purchaseDate,
                source,
                commision,
                isTaxRefund,
                CashTransactionTypes.Commission, transactionLink);
        }
    }


}