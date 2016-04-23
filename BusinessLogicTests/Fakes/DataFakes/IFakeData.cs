using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.BackEnd.Repository.Entities;

namespace BusinessLogicTests.FakeRepositories.DataFakes
{
    public abstract class FakeData
    {
        protected List<FundTransaction> _fundTransactions;
        protected List<Account> _accounts;
        protected List<AccountInvestmentMap> _accountInvestmentMaps;
        protected List<Investment> _investments;
        protected List<PriceHistory> _priceHistories;
        protected List<CashTransaction> _cashTransactions;

        protected FakeData()
        {
            _fundTransactions = new List<FundTransaction>();
            _cashTransactions = new List<CashTransaction>();
            _accounts = new List<Account>();
            _accountInvestmentMaps = new List<AccountInvestmentMap>();
            _investments = new List<Investment>();
            _priceHistories = new List<PriceHistory>();
        }

        public virtual List<Account> Accounts()
        {
            return _accounts;
        }

        public virtual List<AccountInvestmentMap> InvestmentMaps()
        {
            return _accountInvestmentMaps;
        }

        public virtual List<Investment> Investments()
        {
            return _investments;
        }

        public virtual List<FundTransaction> FundTransactions()
        {
            return _fundTransactions;
        }

        public virtual List<CashTransaction> CashTransactions()
        {
            return _cashTransactions;
        }
        

        public virtual List<PriceHistory> PriceHistories()
        {
            return _priceHistories;
        }

    }
    
}
