using System.Web;
using System.Web.Security;

namespace LibrarySystem.Web.StaticFunctionality
{
    public static class StaticFunctions
    {
        public static (int userId, string role)? GetUserFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null) return null;

            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && ticket.UserData.Contains("|"))
                {
                    var parts = ticket.UserData.Split('|');
                    int userId = int.Parse(parts[0]);
                    string role = parts[1];
                    return (userId, role);
                }
            }
            catch
            {
                // Invalid cookie
            }

            return null;
        }
    }
}