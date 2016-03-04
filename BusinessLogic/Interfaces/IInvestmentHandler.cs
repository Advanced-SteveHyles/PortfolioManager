using System.Collections.Generic;
using Portfolio.Common.DTO.DTOs;

namespace Interfaces
{
    public interface IInvestmentHandler
    {
        InvestmentDto GetInvestment(int investmentId);
        IEnumerable<InvestmentDto> GetInvestments();
    }
}