using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface IFundTransactionRepository
    {
        FundTransaction GetFundTransaction(int fundTransactionId);
        RepositoryActionResult<FundTransaction> InsertFundTransaction(CreateFundTransactionRequest request);

    }
}