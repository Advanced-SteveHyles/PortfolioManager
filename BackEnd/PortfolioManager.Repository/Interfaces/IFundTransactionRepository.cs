using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
{
    public interface IFundTransactionRepository
    {
        FundTransaction GetFundTransaction(int fundTransactionId);
        RepositoryActionResult<FundTransaction> InsertFundTransaction(CreateFundTransactionRequest request);

    }
}