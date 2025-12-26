using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(hrms_PakAsia.Startup))]

namespace hrms_PakAsia
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(); // This generates /signalr/hubs dynamically
        }
    }
}
