using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.Repository.Repositories;

namespace Portfolio.BackEnd.BusinessLogic.Factory
{
    public class ProcessorFactory : IProcessorFactory
    {
        public CashTransactionHandler CreateCashTransactionHandler(string connectionString)
        {            
            return new CashTransactionHandler(new CashTransactionRepository(connectionString), new AccountRepository(connectionString) );
        }
    }

    public interface IProcessorFactory
    {
    }
}
