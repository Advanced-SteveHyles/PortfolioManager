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
            _cashTransactions.Add( new CashTransaction() {CashTransactionId = 1, AccountId = 1, TransactionValue = 50});
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 2, AccountId = 1, TransactionValue = 25 });
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 3, AccountId = 1, TransactionValue = 4 });
            _cashTransactions.Add(new CashTransaction() { CashTransactionId = 4, AccountId = 1, TransactionValue = -50 });
        }    
    }
}