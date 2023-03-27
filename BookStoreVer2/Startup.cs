using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookStoreVer2.Startup))]
namespace BookStoreVer2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
