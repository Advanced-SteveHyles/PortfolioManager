using System;
using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace Portfolio.BackEnd.Repository.Repositories
{
    public class InvestmentRepository : BaseRepository, IInvestmentRepository
    {
        public InvestmentRepository(PortfolioManagerContext context) : base(context)
        {
        }

        public IQueryable<Investment> GetInvestments()
        {
            return _context.Investments;
        }

        public Investment GetInvestment(int investmentId)
        {
            return _context.Investments.SingleOrDefault(inv => inv.InvestmentId == investmentId);
        }

        public RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment)
        {
            try
            {
                _context.Investments.Add(entityInvestment);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Investment>(entityInvestment, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Investment>(entityInvestment, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Investment>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}