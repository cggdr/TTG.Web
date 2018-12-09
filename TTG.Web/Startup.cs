using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TTG.Web.Startup))]
namespace TTG.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
