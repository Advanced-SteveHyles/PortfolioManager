using System.Collections.Generic;

namespace Portfolio.Common.Constants.Funds
{
    public static class FundInvestmentTypes
    {
        public const string Tracker = "Tracker";
        public const string Bond = "Bond";
        public const string Fund = "Fund";
        public static List<string> InvestmentTypeList => new List<string>() { Bond, Fund, Tracker};
    }
}