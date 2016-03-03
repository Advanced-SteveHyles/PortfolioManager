namespace PortfolioManager.Repository
{
    public abstract class BaseRepository
    {
        protected PortfolioManagerContext _context;

        protected BaseRepository(PortfolioManagerContext context)
        {
            _context = context;
            _context.Configuration.LazyLoadingEnabled = false;            
        }
    }
}