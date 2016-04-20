using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface ICashTransactionRepository
    {        
        CashTransaction GetCashTransaction(int cashTransactionId);
        IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId);

        RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request);
        
        void ApplyCheckpoint(CashCheckpoint cashCheckpoint);
    }
}