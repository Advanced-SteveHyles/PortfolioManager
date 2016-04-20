using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IInvestmentRepository
    {
        RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment);

        System.Linq.IQueryable<Entities.Investment> GetInvestments();
        Investment GetInvestment(int investmentId);
        
    }
}