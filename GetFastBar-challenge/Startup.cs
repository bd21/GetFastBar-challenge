using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GetFastBar_challenge.Startup))]
namespace GetFastBar_challenge
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
