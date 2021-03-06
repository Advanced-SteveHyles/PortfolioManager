using System.Linq;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Repositories
{
    public class FundTransactionRepository : BaseRepository, IFundTransactionRepository
    {
        public FundTransactionRepository(string connection) : base(new PortfolioManagerContext(connection)) { }
        public FundTransaction GetFundTransaction(int fundTransactionId)
        {
            return _context.FundTransactions.SingleOrDefault(tx => tx.FundTransactionId == fundTransactionId);
        }

        public RepositoryActionResult<FundTransaction> InsertFundTransaction(CreateFundTransactionRequest request)
        {
            var fundTransaction = new FundTransaction()
            {
                InvestmentMapId = request.InvestmentMapId,
                TransactionType = request.TransactionType,
                TransactionDate = request.TransactionDate,
                SettlementDate = request.SettlementDate,
                Source = request.Source,
                Quantity = request.Quantity,
                SellPrice = request.SellPrice,
                BuyPrice = request.BuyPrice,
                Charges = request.Charges,
                TransactionValue = request.TransactionValue,
                LinkedTransactionType = request.LinkedTransactionType,
                LinkedTransaction = request.LinkedTransaction
            };
            
            _context.FundTransactions.Add(fundTransaction);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new RepositoryActionResult<FundTransaction>(fundTransaction, RepositoryActionStatus.Created);
            }
            else
            {
                return new RepositoryActionResult<FundTransaction>(fundTransaction, RepositoryActionStatus.NothingModified, null);
            }
        }
    }
}