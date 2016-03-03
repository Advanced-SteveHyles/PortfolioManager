using System.Collections.Generic;

namespace Portfolio.Common.DTO.DTOs
{
    public class AccountInfoWithAllInvestmentDto
    {
        public AccountDto AccountInfo;

        public ICollection<InvestmentDto> Investments;
    }
}