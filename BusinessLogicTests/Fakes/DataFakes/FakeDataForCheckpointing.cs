using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;

namespace BusinessLogicTests.Fakes.DataFakes
{
    internal class FakeDataForCheckpointing:IFakeData
    {
        public List<Account> FakeAccountData()
        {
            return new List<Account>();
        }

        public List<AccountInvestmentMap> FakePopulatedInvestmentMap()
        {
return new List<AccountInvestmentMap>();
        }

        public List<Investment> FakePopulatedInvestments()
        {
            return new List<Investment>();
        }
    }
}