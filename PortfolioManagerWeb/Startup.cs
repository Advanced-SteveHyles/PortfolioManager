using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortfolioManagerWeb.Startup))]
namespace PortfolioManagerWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
