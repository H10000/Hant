using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using BayNexus.NewsServer.Models;

namespace BayNexus.NewsServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            //app.Map("/signalr", map =>
            //{
            //    var hubConfiguration = new HubConfiguration()
            //    {
            //        EnableJSONP = true
            //    };
            //    map.RunSignalR(hubConfiguration);
            //});
        }
    }
}
