using Hangfire;
using Microsoft.Owin;
using Owin;
using Server;

[assembly: OwinStartup(typeof(Startup))]

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map Dashboard to the `http://<your-app>/hangfire` URL.
            app.UseHangfireDashboard();
        }
    }
}