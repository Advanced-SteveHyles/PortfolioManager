using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests.Transactions;

namespace PortfolioManager.Model
{
    public class AccountInvestmentMapModel
    {
        public static List<AccountInvestmentMapDto> GetInvestments(int accountId)
        {
            var service = new VirtualAccountInvestmentMapController();
            var investmentMaps = service.GetInvestmentsForAccount(accountId);
            return investmentMaps?.ToList() ?? new List<AccountInvestmentMapDto>();
        }

        public static void Buy(InvestmentBuyRequest request)
        {
            var service = new VirtualInvestmentTransactions();
            service.Buy(request);
        }

        public static void ApplyLoyaltyBonus(InvestmentLoyaltyBonusRequest loyaltyBonusRequest)
        {
            var service = new VirtualInvestmentTransactions();
            service.LoyaltyBonus(loyaltyBonusRequest);
        }
    }


}
