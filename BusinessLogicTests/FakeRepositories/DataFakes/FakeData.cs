using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;
using Xunit.Sdk;

namespace BusinessLogicTests.FakeRepositories
{
    internal class FakeData : IFakeData
    {
        List<AccountInvestmentMap> IFakeData.FakePopulatedInvestmentMap()
        {
            return new List<AccountInvestmentMap>
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

        List<Investment> IFakeData.FakePopulatedInvestments()
        {
            throw new NotImplementedException();
        }

        List<Account> IFakeData.FakeAccountData()
        {
            return new List<Account>()
            {
                new Account(){AccountId = 1, Name = "Acc1"},
                new Account(){AccountId = 2, Name = "Acc2"},
                new Account(){AccountId = 3, Name = "Acc3"},
                new Account(){AccountId = 4, Name = "Acc4"},
                new Account(){AccountId = 5, Name = "Acc5"},
                new Account(){AccountId = 6, Name = "Acc6"},                
            };
        }        
    }
}