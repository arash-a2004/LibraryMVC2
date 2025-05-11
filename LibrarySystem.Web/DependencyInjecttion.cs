using LibrarySystem.Application.Services;
using LibrarySystem.Data.Infrastructure;
using LibrarySystem.Domain.Interfaces.Application;
using LibrarySystem.Domain.Interfaces.Repository;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace LibrarySystem.Web
{
    public static class DependencyInjecttion
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register your types here
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();

            // Set the DependencyResolver for MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}