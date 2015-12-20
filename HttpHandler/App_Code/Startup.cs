using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HttpHandler.Startup))]
namespace HttpHandler
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
