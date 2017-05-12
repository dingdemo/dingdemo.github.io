using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ding.Startup))]
namespace Ding
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
