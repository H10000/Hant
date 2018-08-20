using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hant.Web.Startup))]
namespace Hant.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
