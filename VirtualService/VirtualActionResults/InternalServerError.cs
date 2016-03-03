using System.Linq;

namespace Portfolio.API.Virtual.VirtualActionResults
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