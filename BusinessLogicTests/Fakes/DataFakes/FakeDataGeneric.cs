using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;

namespace BusinessLogicTests.Fakes.DataFakes
{
    internal class FakeDataGeneric : FakeData
    {
        public const int FakeInvestmentId = 1;

        private readonly List<Investment> _investments = new List<Investment>()
        {
            new Investment
            {
                InvestmentId =  1,
                Name = "Investment 1"
            }
        };

        private List<Account> _accounts;
        private List<AccountInvestmentMap> _accountInvestmentMaps;

        public FakeDataGeneric()
        {
            _accounts = new List<Account>()
            {
                new Account(){AccountId = 1, Name = "Acc1"},
                new Account(){AccountId = 2, Name = "Acc2"},
                new Account(){AccountId = 3, Name = "Acc3"},
                new Account(){AccountId = 4, Name = "Acc4"},
                new Account(){AccountId = 5, Name = "Acc5"},
                new Account(){AccountId = 6, Name = "Acc6"},                
            };
            _accountInvestmentMaps = new List<AccountInvestmentMap>
            {
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 1,
                    InvestmentId = 1,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 2,
                    InvestmentId = 1,
                    AccountId = 2,
                    Quantity = 5,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 3,
                    InvestmentId = 2,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 4,
                    InvestmentId = 1,
                    AccountId = 3,
                    Quantity = (decimal)25.4,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 88,
                    InvestmentId = 1,
                    AccountId = 4,
                    Quantity = (decimal)1.78923,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 89,
                    InvestmentId = 3,
                    AccountId = 6,
                    Quantity = 21,
                },                
            };
        }


        public override List<AccountInvestmentMap> InvestmentMaps()
        {
            return _accountInvestmentMaps;
        }

        public override List<Investment> Investments()
        {
            return _investments;
        }

        public override List<Account> Accounts()
        {
            return _accounts;
        }        
    }    
}