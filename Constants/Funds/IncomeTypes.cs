using System.Collections.Generic;

namespace Portfolio.Common.Constants.Funds
{
    public static class FundIncomeTypes
    {
        public const string Accumulation = "Accumulation";
        public const string Income = "Income";
        public const string None = "None";
        public static List<string> IncomeTypeList => new List<string>() { Accumulation, Income, None };
    }
}