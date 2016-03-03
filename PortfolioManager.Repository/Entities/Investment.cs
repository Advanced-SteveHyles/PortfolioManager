using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.BackEnd.Repository.Entities
{
    [Table ("Investment")]
    public class Investment
    {
            public int InvestmentId { get; set; }
            public string Name { get; set; }

            public string Symbol { get; set; }

            public string Type { get; set; }
            public string Class { get; set; }
            public string IncomeType { get; set; }
        public string MarketIndex { get; set; }

        
    }
}
