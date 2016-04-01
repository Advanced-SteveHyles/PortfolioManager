using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;

namespace BusinessLogicTests.FakeRepositories
{
    internal class FakeDataForPortfolioValuation : IFakeData
    {

        public const int BondAccountInvestmentMap = 90;
        public const int BondInvestment = 89;
        public const int FundEquityInvestment = 90;
        public const int TrackerEquityInvestment = 91;


        public const int PortfolioWithPropertyAccount = 78;
        public const int PortfolioWithOnlySavingsAccount = 785;
        public const int PortfolioWithNoAccounts = 999;
        public const int PortfolioWithAllAccountTypes = 718;
        public const int PortfolioWithAccountLinkedToBond = 9;

        public const int PropertyAccountForPortfolioWithOnlyPropertyAccount = 7;
        public const int SavingsAccountForPortfolioWithOnlySavingsAccount = 8;
        public const int BondAccountForPortfolioWithOnlyBondsAccounts = 771;

        public const int PropertyAccountForPortfolioWithAllAccountTypes = 1024;
        public const int SavingsAccountForPortfolioWithAllAccountTypes = 1025;
        public const int CashIsaAccountForPortfolioWithAllAccountTypes = 1026;
        public const int PensionAccountForPortfolioWithAllAccountTypes = 1027;
        public const int StockIsaAccountForPortfolioWithAllAccountTypes = 1028;

        public static List<Account> FakeAccountData()
        {
            return new List<Account>()
            {
                new Account() {AccountId = 1, Name = "Acc1"},
                new Account() {AccountId = 2, Name = "Acc2"},
                new Account() {AccountId = 3, Name = "Acc3"},
                new Account() {AccountId = 4, Name = "Acc4"},
                new Account() {AccountId = 5, Name = "Acc5"},
                new Account() {AccountId = 6, Name = "Acc6"},
                new Account()
                {
                    AccountId = PropertyAccountForPortfolioWithOnlyPropertyAccount,
                    Name = "Property Account",
                    Type = PortfolioAccountTypes.Property,
                    PortfolioId = PortfolioWithPropertyAccount
                },
                new Account()
                {
                    AccountId = SavingsAccountForPortfolioWithOnlySavingsAccount,
                    Name = "Savings Account",
                    Type = PortfolioAccountTypes.Savings,
                    PortfolioId = PortfolioWithOnlySavingsAccount
                },
                new Account()
                {
                    AccountId = BondAccountForPortfolioWithOnlyBondsAccounts,
                    Name = "Bond Only Account",
                    Type = PortfolioAccountTypes.StockIsa,
                    PortfolioId = PortfolioWithAccountLinkedToBond
                },

                new Account()
                {
                    AccountId = PropertyAccountForPortfolioWithAllAccountTypes,
                    Name = "Property Account",
                    Type = PortfolioAccountTypes.Property,
                    PortfolioId = PortfolioWithAllAccountTypes
                },
                new Account()
                {
                    AccountId = SavingsAccountForPortfolioWithAllAccountTypes,
                    Name = "Savings Account",
                    Type = PortfolioAccountTypes.Savings,
                    PortfolioId = PortfolioWithAllAccountTypes
                },
                new Account()
                {
                    AccountId = StockIsaAccountForPortfolioWithAllAccountTypes,
                    Name = "Stock Isa",
                    Type = PortfolioAccountTypes.StockIsa,
                    PortfolioId = PortfolioWithAllAccountTypes
                },
                new Account()
                {
                    AccountId = CashIsaAccountForPortfolioWithAllAccountTypes,
                    Name = "Cash Isa",
                    Type = PortfolioAccountTypes.Pension,
                    PortfolioId = PortfolioWithAllAccountTypes
                },
                new Account()
                {
                    AccountId = PensionAccountForPortfolioWithAllAccountTypes,
                    Name = "Pension",
                    Type = PortfolioAccountTypes.CashIsa,
                    PortfolioId = PortfolioWithAllAccountTypes
                },

            };
        }

        List<Account> IFakeData.FakeAccountData()
        {
            return FakeAccountData();
        }

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
                    Quantity = (decimal) 25.4,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 88,
                    InvestmentId = 1,
                    AccountId = 4,
                    Quantity = (decimal) 1.78923,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = 89,
                    InvestmentId = 3,
                    AccountId = 6,
                    Quantity = 21,
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = BondAccountInvestmentMap,
                    InvestmentId = BondInvestment,
                    AccountId = BondAccountForPortfolioWithOnlyBondsAccounts,
                    Quantity = 0,
                    Valuation = 0
                },
            };
        }


        public List<Investment> FakePopulatedInvestments()
        {
            return new List<Investment>()
            {
                new Investment() {InvestmentId = BondInvestment, Type = FundInvestmentTypes.Bond},
                new Investment() {InvestmentId = FundEquityInvestment, Type = FundInvestmentTypes.Fund},
                new Investment() {InvestmentId = TrackerEquityInvestment, Type = FundInvestmentTypes.Tracker},
            };

        }
    }
}