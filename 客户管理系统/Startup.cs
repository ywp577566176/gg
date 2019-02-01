using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(客户管理系统.Startup))]
namespace 客户管理系统
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
