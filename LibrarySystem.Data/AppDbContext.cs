using LibrarySystem.Domain.Models.DbModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LibrarySystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base("Data Source=.;Initial Catalog=LibrarySystemDB;Integrated Security=True;") { }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<Book> Books { get; set; }
    }

}
