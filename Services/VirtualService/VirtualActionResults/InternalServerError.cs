using System.Linq;
using PortfolioManager.Repository.Entities;
using VirtualService.VirtualControllers.API;

namespace VirtualService.VirtualActionResults
{
    public class NotFound : IVirtualActionResult
    {
    }

    public class Ok : IVirtualActionResult
    {

        private IQueryable<Portfolio> portfolios;

        public Ok(IQueryable<Portfolio> portfolios)
        {
            this.portfolios = portfolios;
        }
    }

    public class InternalServerError : IVirtualActionResult
    {        
    }
}