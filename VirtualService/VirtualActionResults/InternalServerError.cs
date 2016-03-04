using System.Linq;

namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class NotFound : IVirtualActionResult
    {
    }

    public class Ok : IVirtualActionResult
    {

        private IQueryable<BackEnd.Repository.Entities.Portfolio> portfolios;

        public Ok(IQueryable<BackEnd.Repository.Entities.Portfolio> portfolios)
        {
            this.portfolios = portfolios;
        }
    }

    public class InternalServerError : IVirtualActionResult
    {        
    }
}