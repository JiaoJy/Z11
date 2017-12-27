using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Z11.Startup))]
namespace Z11
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
