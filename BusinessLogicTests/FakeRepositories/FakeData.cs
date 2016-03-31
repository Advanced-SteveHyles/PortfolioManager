using System.Collections.Generic;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.Constants.TransactionTypes;

namespace BusinessLogicTests.FakeRepositories
{
    internal class FakeData
    {
        public const int PortfolioWithPropertyAccount = 78;
        public const int PortfolioWithoutPropertyAccount = 785;
        public const int PortfolioWithNoAccounts = 999;
        public const int PortfolioWithAllAccountTypes = 718;
        public const int PropertyAccountForPortfolioWithOnlyPropertyAccount = 7;
        public const int SavingsAccountForPortfolioWithOnlySavingsAccount = 8;

        public const int PropertyAccountForPortfolioWithAllAccountTypes = 1024;
        public const int SavingsAccountForPortfolioWithAllAccountTypes = 1025;
        public const int CashIsaAccountForPortfolioWithAllAccountTypes = 1026;
        public const int PensionAccountForPortfolioWithAllAccountTypes = 1027;
        public const int StockIsaAccountForPortfolioWithAllAccountTypes = 1028;

        internal static List<Account> FakeAccountData()
        {
            return new List<Account>()
            {
                new Account(){AccountId = 1, Name = "Acc1"},
                new Account(){AccountId = 2, Name = "Acc2"},
                new Account(){AccountId = 3, Name = "Acc3"},
                new Account(){AccountId = 4, Name = "Acc4"},
                new Account(){AccountId = 5, Name = "Acc5"},
                new Account(){AccountId = 6, Name = "Acc6"},
                new Account(){AccountId = PropertyAccountForPortfolioWithOnlyPropertyAccount, Name = "Property Account", Type = PortfolioAccountTypes.Property, PortfolioId = PortfolioWithPropertyAccount},
                new Account(){AccountId = SavingsAccountForPortfolioWithOnlySavingsAccount, Name = "Savings Account", Type = PortfolioAccountTypes.Savings, PortfolioId = PortfolioWithoutPropertyAccount},

                new Account(){AccountId = PropertyAccountForPortfolioWithAllAccountTypes, Name = "Property Account", Type = PortfolioAccountTypes.Property, PortfolioId = PortfolioWithAllAccountTypes},
                new Account(){AccountId = SavingsAccountForPortfolioWithAllAccountTypes, Name = "Savings Account", Type = PortfolioAccountTypes.Savings, PortfolioId = PortfolioWithAllAccountTypes},
                new Account(){AccountId = StockIsaAccountForPortfolioWithAllAccountTypes, Name = "Stock Isa", Type = PortfolioAccountTypes.StockIsa, PortfolioId = PortfolioWithAllAccountTypes},
                new Account(){AccountId = CashIsaAccountForPortfolioWithAllAccountTypes, Name = "Cash Isa", Type = PortfolioAccountTypes.Pension, PortfolioId = PortfolioWithAllAccountTypes},
                new Account(){AccountId = PensionAccountForPortfolioWithAllAccountTypes, Name = "Pension", Type = PortfolioAccountTypes.CashIsa, PortfolioId = PortfolioWithAllAccountTypes},

            };
        }

        internal static List<AccountInvestmentMap> FakePopulatedInvestmentMap()
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
    }
}