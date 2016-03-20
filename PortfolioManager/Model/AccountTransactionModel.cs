using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.Requests.Transactions;

namespace PortfolioManager.Model
{
    class AccountTransactionModel
    {
        public static void InsertDeposit(DepositTransactionRequest request)
        {
            var service = new VirtualCashTransactions();
            service.InsertDeposit(request);
        }
    }
}
