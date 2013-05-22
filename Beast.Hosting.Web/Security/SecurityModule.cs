using System;
using System.Web;
using System.Web.Security;

namespace Beast.Hosting.Web.Security
{
    public class SecurityModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += OnPostAuthenticateRequest;
        }

        void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null)
                return;
            var user = SecurityManager.CreatePrincipal(app.Context);
            if (user != null)
                app.Context.User = user;
        }
    }
}
