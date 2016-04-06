using System.Collections.Generic;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.Constants.TransactionTypes;

namespace BusinessLogicTests.FakeRepositories
{
    internal class FakeDataForPortfolioValuation : IFakeData
    {

        public const int PortfolioWithNoAccounts = 999;

        public const int BondAccountInvestmentMap = 90;
        public const int EquityAccountInvestmentMap = 900;
        public const int StockIsaAccountMap = 910;

        public const int BondInvestment = 89;
        public const int FundEquityInvestment = 90;
        public const int TrackerEquityInvestment = 91;


        public const int PortfolioWithPropertyOnlyAccount = 78;
        public const int PortfolioWithOnlySavingsAccount = 785;
        public const int PortfolioWithAccountLinkedToBond = 9;
        public const int PortfolioWithAccountLinkedToEquity = 99;

        public const int PortfolioWithAllAccountTypes = 718;
        

        public const int PropertyAccountForPortfolioWithOnlyPropertyAccount = 7;
        public const int SavingsAccountForPortfolioWithOnlySavingsAccount = 8;
        public const int BondAccountForPortfolioWithOnlyBondsAccounts = 771;
        public const int EquityAccountForPortfolioWithOnlyEquityAccounts = 772;

        public const int PropertyAccountForPortfolioWithAllAccountTypes = 1024;
        public const int SavingsAccountForPortfolioWithAllAccountTypes = 1025;
        public const int CashIsaAccountForPortfolioWithAllAccountTypes = 1026;
        public const int PensionAccountForPortfolioWithAllAccountTypes = 1027;
        public const int StockIsaAccountForPortfolioWithAllAccountTypes = 1028;

        List<Account> IFakeData.FakeAccountData()
        {
            return new List<Account>()
            {                
                new Account()
                {
                    AccountId = PropertyAccountForPortfolioWithOnlyPropertyAccount,
                    Name = "Property Account",
                    Type = PortfolioAccountTypes.Property,
                    PortfolioId = PortfolioWithPropertyOnlyAccount
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
                    AccountId = EquityAccountForPortfolioWithOnlyEquityAccounts,
                    Name = "Equity Only Account",
                    Type = PortfolioAccountTypes.StockIsa,
                    PortfolioId = PortfolioWithAccountLinkedToEquity
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
                    Type = PortfolioAccountTypes.CashIsa,
                    PortfolioId = PortfolioWithAllAccountTypes
                },
                new Account()
                {
                    AccountId = PensionAccountForPortfolioWithAllAccountTypes,
                    Name = "Pension",
                    Type = PortfolioAccountTypes.Pension,
                    PortfolioId = PortfolioWithAllAccountTypes
                },

            };
        }

        List<AccountInvestmentMap> IFakeData.FakePopulatedInvestmentMap()
        {
            return new List<AccountInvestmentMap>
            {
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = BondAccountInvestmentMap,
                    InvestmentId = BondInvestment,
                    AccountId = BondAccountForPortfolioWithOnlyBondsAccounts,
                    Quantity = 0,
                    Valuation = 0
                },
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = EquityAccountInvestmentMap,
                    InvestmentId = FundEquityInvestment,
                    AccountId = EquityAccountForPortfolioWithOnlyEquityAccounts,
                    Quantity = 0,
                    Valuation = 0
                },                
                new AccountInvestmentMap()
                {
                    AccountInvestmentMapId = StockIsaAccountMap,
                    InvestmentId = FundEquityInvestment,
                    AccountId = StockIsaAccountForPortfolioWithAllAccountTypes,
                    Quantity = 0,
                    Valuation = 0
                },
            };
        }


        public List<Investment> FakePopulatedInvestments()
        {
            return new List<Investment>()
            {
                new Investment() {InvestmentId = BondInvestment, Type = FundInvestmentTypes.Bond, Class = FundClasses.Oeic},
                new Investment() {InvestmentId = FundEquityInvestment, Type = FundInvestmentTypes.Fund, Class = FundClasses.Oeic},
                new Investment() {InvestmentId = TrackerEquityInvestment, Type = FundInvestmentTypes.Tracker},
            };

        }
    }
}