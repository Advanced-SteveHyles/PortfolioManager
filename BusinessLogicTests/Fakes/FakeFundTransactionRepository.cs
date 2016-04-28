using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace BusinessLogicTests.Fakes
{
    public class FakeFundTransactionRepository : IFundTransactionRepository
    {
        private int _nextFundTransactionId;

        private readonly FakeData _fakeData;
        public FakeFundTransactionRepository(FakeData fakeData)
        {
            _fakeData = fakeData;
        }

        public FundTransaction GetFundTransaction(int fundTransactionId)
        {
            return _fakeData.FundTransactions().Single(t => t.FundTransactionId == fundTransactionId);
        }


        public RepositoryActionResult<FundTransaction> InsertFundTransaction(CreateFundTransactionRequest request)
        {
            _nextFundTransactionId++;
            var dummyFundTransaction = new FundTransaction()
            {
                FundTransactionId = _nextFundTransactionId,

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

            _fakeData.FundTransactions().Add(dummyFundTransaction);

            return new RepositoryActionResult<FundTransaction>(dummyFundTransaction, RepositoryActionStatus.Ok);
        }

    }
}