using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Template.Startup))]
namespace Web_Template
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
