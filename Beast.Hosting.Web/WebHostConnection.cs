using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Beast.Hosting.Web
{
    public class WebHostConnection : PersistentConnection
    {
        public const string DefaultSignalRRouteName = "beast_signalr";
        public const string DefaultSignalRUrl = "/server";

        private Application _app;

        public WebHostConnection(Application app)
        {
            _app = app;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var conn = new SignalRConnection
            {
                Id = connectionId
            };

            return base.OnConnected(request, connectionId);
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return base.OnReconnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            return base.OnDisconnected(request, connectionId);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return base.OnReceived(request, connectionId, data);
        }
    }
}
