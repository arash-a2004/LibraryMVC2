using System.Web.Mvc;
using System.Web.Security;

namespace LibrarySystem.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (FormsIdentity)User.Identity;
                var ticket = identity.Ticket;
                var role = ticket.UserData; // ← this is where we stored the role

                switch (role)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                    case "Librarian":
                        return RedirectToAction("Index", "Librarian");
                    default:
                        return RedirectToAction("Index", "User");
                }
            }

            return RedirectToAction("Login", "Account");
        }

    }
}