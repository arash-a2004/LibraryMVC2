using LibrarySystem.Infrastructure.Infra;
using LibrarySystem.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LibrarySystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Dependency Injection
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register repositories
            services.AddScoped<IAccountingRepository, AccountingRepository>();

            // Register services

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();

            // Set the DependencyResolver for MVC
            DependencyResolver.SetResolver(new DefaultDependencyResolver(ServiceProvider));
        }
    }

    // Custom DependencyResolver to integrate with Microsoft.Extensions.DependencyInjection
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            return service is IEnumerable<object> enumerable ? enumerable : new[] { service };
        }
    }
}
