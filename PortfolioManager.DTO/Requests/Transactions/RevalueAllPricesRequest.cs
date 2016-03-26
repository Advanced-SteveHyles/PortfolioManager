using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class RevalueAllPricesRequest : ITransactionRequest
    {
        public DateTime EvaluationDate { get; set; }
    }
}