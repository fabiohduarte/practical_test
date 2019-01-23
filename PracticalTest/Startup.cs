using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PracticalTest.Startup))]
namespace PracticalTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
