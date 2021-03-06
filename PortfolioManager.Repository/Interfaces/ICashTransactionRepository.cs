using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface ICashTransactionRepository
    {        
        CashTransaction GetCashTransactionById(int cashTransactionId);
        IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId);

        RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request);
        
        RepositoryActionResult<CashTransaction> ApplyCheckpoint(CashCheckpoint cashCheckpoint, int transactionId);
    }
}