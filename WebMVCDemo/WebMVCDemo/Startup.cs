using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebMVCDemo.Startup))]
namespace WebMVCDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
