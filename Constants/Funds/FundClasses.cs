using System.Collections.Generic;

namespace Portfolio.Common.Constants.Funds
{
    public static class FundClasses
    {
        public const string Oeic = "OEIC";
        public const string UnitTrust = "Unit Trust";
        public static List<string> FundClassList => new List<string>() { Oeic , UnitTrust };

    }
}
