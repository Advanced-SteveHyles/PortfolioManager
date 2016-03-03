using System;
using System.Linq;
using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository
{
    public class CashTransactionRepository : BaseRepository, ICashTransactionRepository
    {
        public CashTransactionRepository(PortfolioManagerContext context): base(context)
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
                TransactionType = request.TransactionType
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