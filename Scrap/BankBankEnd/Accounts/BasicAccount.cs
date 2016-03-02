using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBankEnd.Accounts
{
    public  class BasicAccount :  IAccount
    {
        List<ITransaction> _Transactions;
        public List<ITransaction> Transactions
        {
            get
            {
                if (_Transactions == null)
                {
                    _Transactions = new List<ITransaction>();
                }
                return _Transactions;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public decimal PredictedBalance { get; set; }
        public decimal ActualBalance { get; set; }

        public void UpdateBalances()
        {
          
        }

    }
}
