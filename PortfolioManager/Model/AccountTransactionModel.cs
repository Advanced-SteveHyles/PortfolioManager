using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual.VirtualControllers;
using Portfolio.Common.DTO.Requests.Transactions;

namespace PortfolioManager.Model
{
    internal class AccountTransactionModel
    {
        public static void InsertDeposit(DepositTransactionRequest request)
        {
            var service = new VirtualCashTransactions();
            service.InsertDeposit(request);
        }

        public static void InsertWithdrawal(WithdrawalTransactionRequest request)
        {
            var service = new VirtualCashTransactions();
            service.InsertWithdrawal(request);
        }

        public static void InsertFee(FeeTransactionRequest request)
        {
            var service = new VirtualCashTransactions();
            service.InsertFee(request);
        }

        public static void InsertTransfer(CashTransferRequest request)
        {
            var service = new VirtualCashTransactions();
            service.InsertTransfer(request);            
        }
    }
}
