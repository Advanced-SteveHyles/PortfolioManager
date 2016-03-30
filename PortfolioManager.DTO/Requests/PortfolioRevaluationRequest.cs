namespace Portfolio.Common.DTO.Requests
{
    public class PortfolioRevaluationRequest : ITransactionRequest
    {
        public int PortfolioId { get; set; }
    }
}