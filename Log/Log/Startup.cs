using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Log.Startup))]
namespace Log
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
