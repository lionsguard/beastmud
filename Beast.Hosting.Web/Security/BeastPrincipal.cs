using System.Security.Principal;

namespace Beast.Hosting.Web.Security
{
    public class BeastPrincipal : IPrincipal
    {
        public BeastIdentity Identity { get; private set; }

        public BeastPrincipal(BeastIdentity identity)
        {
            Identity = identity;
        }

        IIdentity IPrincipal.Identity
        {
            get { return Identity; }
        }

        bool IPrincipal.IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated;
        }
    }
}
