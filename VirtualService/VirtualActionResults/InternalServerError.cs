using System.Linq;

namespace Portfolio.API.Virtual.VirtualActionResults
{
    public class NotFound : IVirtualActionResult
    {
    }

    public class Ok<T> : IVirtualActionResult
    {
        private IQueryable<T> _objects;

        public Ok(IQueryable<T> objects)
        {
            _objects = objects;
        }

        public IQueryable<T> objects => _objects;

    }

    public class InternalServerError : IVirtualActionResult
    {        
    }
}