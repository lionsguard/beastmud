using System.Security.Principal;

namespace Beast.Hosting.Web.Security
{
    public static class SecurityExtensions
    {
        public static BeastIdentity ToBeastIdentity(this IIdentity identity)
        {
            if (identity.GetType() == typeof(BeastIdentity))
                return (BeastIdentity)identity;

            return BeastIdentity.FromIdentity(identity);
        }
    }
}
