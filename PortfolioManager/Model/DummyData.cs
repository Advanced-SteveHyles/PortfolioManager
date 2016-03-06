using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.Common.DTO.DTOs;

namespace PortfolioManager.Model
{
    public static class DummyData
    {
        public static List<PortfolioDto> GetPortfolioList() => new List<PortfolioDto>()
        {
            {new PortfolioDto() { Name = "Portfolio One", PortfolioId = 1}
            },
            {new PortfolioDto() { Name = "Portfolio Two", PortfolioId = 2}
            },
            {new PortfolioDto() { Name = "Portfolio Three", PortfolioId = 3}},
        };

        internal static List<AccountDto> FakeAccountData()
        {
            return new List<AccountDto>()
            {
                new AccountDto(){AccountId = 1, PortfolioId = 1, Name = "Account A"},
                new AccountDto(){AccountId = 2, PortfolioId = 1, Name = "Account V"},
                new AccountDto(){AccountId = 3, PortfolioId = 1, Name = "Account G"},
                new AccountDto(){AccountId = 4, PortfolioId = 2, Name = "Account A1"},
                new AccountDto(){AccountId = 5, PortfolioId = 2, Name = "Account A"},
                new AccountDto(){AccountId = 6, PortfolioId = 3, Name = "Account A3"}
            };
        }

        internal static List<AccountInvestmentMapDto> FakePopulatedInvestmentMap()
        {
            return new List<AccountInvestmentMapDto>
            {
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 1,
                    InvestmentId = 1,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 2,
                    InvestmentId = 1,
                    AccountId = 2,
                    Quantity = 5,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 3,
                    InvestmentId = 2,
                    AccountId = 1,
                    Quantity = 10,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 4,
                    InvestmentId = 1,
                    AccountId = 3,
                    Quantity = (decimal)25.4,
                },
                new AccountInvestmentMapDto()
                {
                    AccountInvestmentMapId = 88,
                    InvestmentId = 1,
                    AccountId = 4,
                    Quantity = (decimal)1.78923,
                },
                new AccountInvestmentMapDto()
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
