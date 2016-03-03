using Portfolio.API.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace Portfolio.API.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}
