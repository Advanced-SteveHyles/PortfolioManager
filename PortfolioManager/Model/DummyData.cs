using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.Common.DTO.DTOs;

namespace PortfolioManager.Model
{
    public static class DummyData
    {
        public static List<PortfolioDto> GetPortfolioList() => new List<PortfolioDto>()
        {
            {new PortfolioDto() { Name = "Portfolio One"}
            },
            {new PortfolioDto() { Name = "Portfolio Two"}
            },
            {new PortfolioDto() { Name = "Portfolio Three"}},
        } ;
        
    }
}
