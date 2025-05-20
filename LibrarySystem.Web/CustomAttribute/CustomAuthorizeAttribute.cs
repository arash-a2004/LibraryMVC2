using System.Web;
using System.Web.Mvc;

namespace LibrarySystem.Web.CustomAttribute
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = StaticFunctionality.StaticFunctions.GetUserFromCookie();
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
                return;
            }

            // Store in HttpContext for use inside controller
            HttpContext.Current.Items["UserId"] = user.Value.userId;
            HttpContext.Current.Items["UserRole"] = user.Value.role;

            base.OnActionExecuting(filterContext);
        }
    }

}