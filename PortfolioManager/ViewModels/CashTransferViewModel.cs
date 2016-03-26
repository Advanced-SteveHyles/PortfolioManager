using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests.Transactions;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class CashTransferViewModel : AbstractSaveCancelCommands
    {
        public List<AccountDto> AccountsFrom { get; set; }
        public List<AccountDto> AccountsTo { get; set; }

        public decimal TransferAmount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        public AccountDto SelectedFromAccount { get; set; }

        public AccountDto SelectedToAccount { get; set; }
        
            private int accountId;
        private readonly Action completeTransaction;

        public static CashTransferViewModel CreateCashTransferOutViewModel(int accountId, Action completeTransaction)
        {
            var account = LoadAccount(accountId);
            var accountsFrom = new List<AccountDto>() { account };            
            var accountsTo = LoadAccountsList(account.PortfolioId, accountId);
            return new CashTransferViewModel(accountId, completeTransaction, accountsFrom, accountsTo);
        }

        public static CashTransferViewModel CreateCashTransferInViewModel(int accountId, Action completeTransaction)
        {
            var account = LoadAccount(accountId);            
            var accountsFrom = LoadAccountsList(account.PortfolioId, accountId); 
            var accountsTo = new List<AccountDto>() { account };
            return new CashTransferViewModel(accountId, completeTransaction, accountsFrom, accountsTo);
        }

        private static AccountDto LoadAccount(int id)
        {
            return AccountModel.GetAccount(id);
        }

        private static List<AccountDto> LoadAccountsList(int portfolioId, int accountId)
        {
            return AccountModel.GetAccountForPortfolio(portfolioId).Where(account => account.AccountId != accountId).ToList();
        }

        private CashTransferViewModel(int accountId, Action completeTransaction, List<AccountDto> accountsFrom, List<AccountDto> accountsTo)
        {
            this.accountId = accountId;
            this.completeTransaction = completeTransaction;
            AccountsFrom = accountsFrom;
            AccountsTo = accountsTo;
            SetCommands(Save,Cancel);

            SelectedFromAccount = accountsFrom.FirstOrDefault();
            SelectedToAccount = accountsTo.FirstOrDefault();
        }

        private void Cancel()
        {
            completeTransaction.Invoke();
        }

        private void Save()
        {
            var cashTransferRequest = new CashTransferRequest()
            {
        FromAccount = SelectedFromAccount?.AccountId ?? 0,
        ToAccount = SelectedToAccount?.AccountId ?? 0,
        Amount = TransferAmount,
        TransactionDate = TransactionDate
            }
            ;
            AccountTransactionModel.InsertTransfer(cashTransferRequest);

            completeTransaction.Invoke();
        }
    }


}