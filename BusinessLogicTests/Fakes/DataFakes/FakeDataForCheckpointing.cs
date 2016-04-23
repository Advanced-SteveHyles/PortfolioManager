using System.Collections.Generic;
using System.Linq.Expressions;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;

namespace BusinessLogicTests.Fakes.DataFakes
{
    internal class FakeDataForCheckpointing: FakeData
    {
        public FakeDataForCheckpointing()
        {
            _cashTransactions.Add( new CashTransaction() {CashTransactionId = 1});
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 2 });
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 3 });
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 4 });
        }    
    }
}