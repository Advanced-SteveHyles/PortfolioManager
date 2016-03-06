using System.Collections.Generic;
using System.Linq;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class OkMultipleActionResult<T> : IVirtualActionResult
    {
        public IEnumerable<PortfolioDto> EnumerateObjectInstances { get; }
        public IQueryable<T> QueryObjectInstances { get; }

        public OkMultipleActionResult(IEnumerable<PortfolioDto> enumerateObjectInstances)
        {
            this.EnumerateObjectInstances = enumerateObjectInstances;
        }

        public OkMultipleActionResult(IQueryable<T> queryObjectInstances)
        {
            QueryObjectInstances = queryObjectInstances;
        }
        
    }
}