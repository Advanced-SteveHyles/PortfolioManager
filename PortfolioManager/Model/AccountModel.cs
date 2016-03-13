using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualActionResults;
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
            var accounts = service.Get(portfolioId) as OkMultipleActionResult<AccountDto>;
            return accounts?.EnumerateObjectInstances.ToList() ?? new List<AccountDto>();
        }

        public static List<CashTransactionDto> GetAccountTransactions(int accountId)
        {
            var service = new VirtualAccountSummaryController();
            var accounts = service.GetCashTransactionSummary(accountId) as OkMultipleActionResult<CashTransactionDto>;
            return accounts?.EnumerateObjectInstances.ToList() ?? new List<CashTransactionDto>();
        }
    }
}