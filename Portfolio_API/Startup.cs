using Microsoft.Owin;
using Owin;
using Portfolio.API.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace Portfolio.API.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}
