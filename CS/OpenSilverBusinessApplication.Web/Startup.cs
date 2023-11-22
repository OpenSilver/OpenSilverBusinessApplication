using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OpenSilverBusinessApplication.Web.Startup))]

namespace OpenSilverBusinessApplication.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}