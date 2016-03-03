using System.Collections.Generic;

namespace Portfolio.Common.DTO.DTOs
{
    public class PortfolioDto
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public ICollection<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }    
}
