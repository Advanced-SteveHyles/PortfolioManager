using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.BackEnd.Repository.Entities;

namespace BusinessLogicTests.FakeRepositories.DataFakes
{
    public interface IFakeData
    {
        List<Account> FakeAccountData();
        List<AccountInvestmentMap> FakePopulatedInvestmentMap();
        List<Investment> FakePopulatedInvestments();
    }
    
}
