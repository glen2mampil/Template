using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SysDev.Startup))]
namespace SysDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
