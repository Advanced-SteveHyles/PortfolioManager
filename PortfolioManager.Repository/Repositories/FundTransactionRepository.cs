using System.Linq;
using PortfolioManager.DTO.Requests;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace PortfolioManager.Repository.Repositories
{
    public class FundTransactionRepository : BaseRepository, IFundTransactionRepository
    {
        public FundTransactionRepository(PortfolioManagerContext context) : base(context) { }
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
                TransactionValue = request.TransactionValue
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