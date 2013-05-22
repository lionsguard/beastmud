using System.Security.Principal;

namespace Beast.Hosting.Web.Security
{
    public class BeastIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { return "BeastIdentity"; }
        }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }
        public string ConnectionId { get; private set; }
        public bool PersistLogin { get; private set; }

        public BeastIdentity(string name, string connectionId, bool persistLogin)
        {
            Name = name;
            ConnectionId = connectionId;
            PersistLogin = persistLogin;
            IsAuthenticated = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ConnectionId);
        }

        private BeastIdentity(string name, bool isAuthenticated)
        {
            Name = name;
            IsAuthenticated = IsAuthenticated;
        }

        public static BeastIdentity FromIdentity(IIdentity identity)
        {
            return new BeastIdentity(identity.Name, identity.IsAuthenticated);
        }
    }
}
