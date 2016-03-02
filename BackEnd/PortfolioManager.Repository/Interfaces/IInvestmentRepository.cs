using PortfolioManager.DTO;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
{
    public interface IInvestmentRepository
    {
        RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment);

        System.Linq.IQueryable<Entities.Investment> GetInvestments();
        Investment GetInvestment(int investmentId);
    }
}