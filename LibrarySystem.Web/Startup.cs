using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartup(typeof(LibrarySystem.Web.Startup))]

namespace LibrarySystem.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure OWIN to use ASP.NET MVC
            ConfigureMvc();
        }

        private void ConfigureMvc()
        {
            // Register MVC routes
            RouteTable.Routes.MapMvcAttributeRoutes();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
