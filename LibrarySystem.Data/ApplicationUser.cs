using Microsoft.AspNet.Identity.EntityFramework;

namespace LibrarySystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

}
