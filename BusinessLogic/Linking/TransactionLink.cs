using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.BackEnd.BusinessLogic.Linking
{
    public class TransactionLink
    {
        private enum LinkTypes
        {
            FundToFund,
            FundToCash,
            CashToCash,
        }

        public Guid LinkedTransaction { get; }
        public string LinkedTransactionType { get; }

        public static TransactionLink FundToCash()
        {
            return new TransactionLink(LinkTypes.FundToCash);
        }

        public static TransactionLink CashToCash()
        {
            return new TransactionLink(LinkTypes.CashToCash);
        }

        private TransactionLink(LinkTypes linkType)
        {
            LinkedTransaction = Guid.NewGuid();
            LinkedTransactionType = linkType.ToString();
        }

        
    }
}
