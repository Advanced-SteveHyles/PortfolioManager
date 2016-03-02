using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.DTO
{
    public class PortfolioDto
    {
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public ICollection<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }    
}
