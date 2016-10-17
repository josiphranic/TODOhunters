using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(urednistvo.Startup))]
namespace urednistvo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
