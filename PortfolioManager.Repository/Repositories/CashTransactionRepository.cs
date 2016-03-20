using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Repositories
{
    public class CashTransactionRepository : BaseRepository, ICashTransactionRepository
    {
        public CashTransactionRepository(string connection): base(new PortfolioManagerContext(connection))
        {        
        }

        public RepositoryActionResult<CashTransaction> InsertCashTransaction(CreateCashTransactionRequest request)
        {
            var entityTransaction = new CashTransaction()
            {
                AccountId = request.AccountId,
                TransactionDate = request.TransactionDate,
                Source = request.Source,
                TransactionValue = request.TransactionValue,
                IsTaxRefund = request.IsTaxRefund,
                TransactionType = request.TransactionType,
                LinkedTransaction = request.LinkedTransaction,
                LinkedTransactionType = request.LinkedTransactionType
            };

           _context.CashTransactions.Add(entityTransaction);

            var result = _context.SaveChanges();
            if (result > 0)
            {
                return new RepositoryActionResult<CashTransaction>(entityTransaction, RepositoryActionStatus.Created);
            }
            else
            {
                return new RepositoryActionResult<CashTransaction>(entityTransaction, RepositoryActionStatus.NothingModified, null);
            }
        }

        public CashTransaction GetCashTransaction(int cashTransactionId)
        {
            return _context.CashTransactions.SingleOrDefault(ct => ct.CashTransactionId == cashTransactionId);
        }

        public IQueryable<CashTransaction> GetCashTransactionsForAccount(int accountId)
        {
            var tx = _context.CashTransactions.Where(t => t.AccountId == accountId);
            return tx;
        }
    }
}