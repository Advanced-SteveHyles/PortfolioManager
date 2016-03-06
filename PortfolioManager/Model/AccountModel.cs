using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;

namespace PortfolioManager.Model
{
    public static class AccountModel
    {
        internal static List<AccountDto> GetAccountForPortfolio(int portfolioId)
        {
            var service = new VirtualAccountController(ApiConstants.VirtualApiPortfoliomanagercontext);
            var accounts = service.Get(portfolioId) as OkMultipleActionResult<AccountDto>;
            return accounts?.EnumerateObjectInstances.ToList() ?? new List<AccountDto>();
        }
    }
}