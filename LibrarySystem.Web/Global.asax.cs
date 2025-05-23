﻿using System.Web.Mvc;
using System.Web.Routing;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services.Accounting;
using LibrarySystem.Application.Services.AdminServices;
using LibrarySystem.Application.Services.FineCheckerServices;
using LibrarySystem.Application.Services.LibrarianServices;
using LibrarySystem.Application.Services.MemberServices;
using LibrarySystem.Infrastructure.Infra;
using LibrarySystem.Infrastructure.Interfaces;
using Unity;
using Unity.Mvc5;

namespace LibrarySystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Create the Unity container
            var container = new UnityContainer();

            // Register types (services and interfaces)
            RegisterTypes(container);

            // Set the Dependency Resolver to use Unity
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Other configuration steps for MVC
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        // Method to register types in Unity
        private void RegisterTypes(IUnityContainer container)
        {
            // Register services with Unity container
            container.RegisterType<IAccountingRepository, AccountingRepository>();
            container.RegisterType<IUserrepository, Userrepository>();
            container.RegisterType<IAccountingServices, AccountingServices>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<ILibrarianRepository, LibrarianRepository>();
            container.RegisterType<ILibrarianService, LibarianService>();
            container.RegisterType<IMemberRepository, MemberRepository>();
            container.RegisterType<IMemberServices, MemberServices>();
            container.RegisterType<IFineCheckerRepository, FineCheckerRepository>();
            container.RegisterType<IFineCheckerServices, FineCheckerServices>();
        }

    }
}
