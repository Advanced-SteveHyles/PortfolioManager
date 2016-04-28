using System;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.Fakes
{
    public class FakeInvestmentRepository :
        IInvestmentRepository
    {
        private readonly FakeData _fakeData;

        public FakeInvestmentRepository(FakeData fakeData)
        {
            _fakeData = fakeData;
        }

        public RepositoryActionResult<Investment> InsertInvestment(Investment entityInvestment)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Investment> GetInvestments()
        {
            return _fakeData.InvestmentMaps().Select(inv => new Investment()
            {
                InvestmentId = inv.InvestmentId,
            }).AsQueryable();
        }

        public Investment GetInvestment(int investmentId)
        {
            return _fakeData.Investments().Single(inv => inv.InvestmentId == investmentId);
        }

    }
}
