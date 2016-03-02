using System.Collections.Generic;

namespace PortfolioManager.DTO.DTOs
{
    public class AccountInfoWithAllInvestmentDto
    {
        public AccountDto AccountInfo;

        public ICollection<InvestmentDto> Investments;
    }
}