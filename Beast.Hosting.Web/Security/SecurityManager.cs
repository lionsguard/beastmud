using System;
using System.Web;
using System.Web.Security;

namespace Beast.Hosting.Web.Security
{
    public static class SecurityManager
    {
        public static void SignIn(string userName, string connectionId, bool persistLogin, HttpCookieCollection cookies)
        {
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.UtcNow, DateTime.UtcNow.Add(FormsAuthentication.Timeout), persistLogin, connectionId);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            cookies.Add(cookie);
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public static BeastPrincipal CreatePrincipal(HttpContext context)
        {
            if (context != null)
            {
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);

                    var identity = new BeastIdentity(ticket.Name, ticket.UserData, ticket.IsPersistent);
                    return new BeastPrincipal(identity);
                }
            }
            return null;
        }
    }
}
