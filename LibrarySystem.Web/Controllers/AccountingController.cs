using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;

namespace LibrarySystem.Web.Controllers
{
    public class AccountingController : Controller
    {
        private readonly IAccountingServices _accountingServices;
        public AccountingController(IAccountingServices accountingServices)
        {
            _accountingServices = accountingServices;
        }

        // GET: Accounting
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserSignUpDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = _accountingServices.SignUp(model); // Your logic to add user in DB

                if (result == null)
                {
                    ModelState.AddModelError("", "can not register now");
                    return View(model);
                }

                TempData["Success"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "can not register now");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserSignInDto model)
        {
            if (ModelState.IsValid)
            {
                // Example: user retrieved from DB
                var user = await _accountingServices.SignIn(model);

                if (user != null)
                {
                    // Create authentication ticket (Forms Authentication)
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        user.Username,                 // user identifier
                        DateTime.Now,
                        DateTime.Now.AddDays(30),  // expiry
                        false,                         // isPersistent
                        user.Role.ToString()           // store role in UserData
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ModelState.AddModelError("", "Invalid login.");
            return View(model);
        }

    }
}