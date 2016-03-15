using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace PortfolioManager.Model
{
    public static class AccountModel
    {
        internal static List<AccountDto> GetAccountForPortfolio(int portfolioId)
        {
            var service = new VirtualAccountController();
            var accounts = service.GetAccountsForPortfolio (portfolioId);
            return accounts?.ToList() ?? new List<AccountDto>();
        }

        public static List<CashTransactionDto> GetAccountTransactions(int accountId)
        {
            var service = new VirtualAccountSummaryController();
            var accounts = service.GetCashTransactionSummary(accountId);
            return accounts?.ToList() ?? new List<CashTransactionDto>();
        }

        public static AccountDto GetAccount(int accountId)
        {
            var service = new VirtualAccountController();
            var account = service.GetAccount(accountId) ;
            return account;
        }
    }
}