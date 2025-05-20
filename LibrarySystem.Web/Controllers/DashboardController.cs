using System.Web.Mvc;
using System.Web.Security;
using LibrarySystem.Web.CustomAttribute;

namespace LibrarySystem.Web.Controllers
{
    [CustomAuthorize]
    public class DashboardController : Controller
    {
        protected int CurrentUserId => (int)HttpContext.Items["UserId"];
        protected string CurrentUserRole => HttpContext.Items["UserRole"].ToString();
        // GET: Dashboard
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (FormsIdentity)User.Identity;
                var ticket = identity.Ticket;
                var role = CurrentUserRole; // ← this is where we stored the role

                switch (role)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                    case "Librarian":
                        return RedirectToAction("Index", "Librarian");
                    case "FineChecker":
                        return RedirectToAction("Index", "FineChecker");
                    default:
                        return RedirectToAction("Index", "Member");
                }
            }

            return RedirectToAction("Login", "Account");
        }

    }
}