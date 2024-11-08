using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GuardianAngels.Startup))]
namespace GuardianAngels
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
