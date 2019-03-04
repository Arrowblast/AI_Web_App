using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AI_Web_App.Startup))]
namespace AI_Web_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
