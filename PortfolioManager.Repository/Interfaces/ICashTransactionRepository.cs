using System.Linq;
using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository.Interfaces
{
    public interface ICashTransactionRepository
    {        
        CashTransaction GetCashTransaction(int cashTransactionId);
        IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId);

        RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request);

    }
}