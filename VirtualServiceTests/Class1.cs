using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualService.VirtualActionResults;
using Xunit;
namespace VirtualServiceTests
{
    public class PortfolioServiceTests
    {
        [Fact]
        public void CallsToPortfolioServiceSucceed()
        {
            var connection = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=c:\temp\PortfolioManagerDummy.mdf;Initial Catalog=aspnet-Portfolio_API-20160118080906;Integrated Security=True";
            var sut = new VirtualService.VirtualControllers.PortfoliosController(connection);
            var portfolios =  sut.Get();
            Assert.IsType<Ok> (portfolios);
        }
    }
}
