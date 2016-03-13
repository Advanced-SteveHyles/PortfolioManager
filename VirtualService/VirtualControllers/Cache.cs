using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Repositories;

namespace Portfolio.API.Virtual.VirtualControllers
{
    internal class Cache
    {
        static readonly Dictionary<int, string> InvestmentNames = new Dictionary<int, string> ();

        internal static string GetInvestmentName(int investmentId)
        {
            if (!InvestmentNames.ContainsKey(investmentId))
            {
                var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
                var repository = new InvestmentRepository(connection);
                var investment = repository.GetInvestment(investmentId);
                InvestmentNames.Add(investment.InvestmentId, investment.Name);

                return InvestmentNames[investmentId];
            }
            return InvestmentNames[investmentId];
        }
    }
}