using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Beast.Json;
using Beast.IO;

namespace Beast.Hosting.Web
{
    public class WebHostConnection : PersistentConnection
    {
        public const string DefaultSignalRRouteName = "beast_signalr";
        public const string DefaultSignalRUrl = "/server";

        protected Application App { get; private set; }

        public WebHostConnection(Application app)
        {
            App = app;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var conn = new SignalRConnection
            {
                Id = connectionId
            };

            App.AddConnection(conn);

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
            var conn = App.FindConnection(connectionId);
            if (conn != null)
            {
                App.ProcessInput(conn, InputResolver.Resolve(data));
            }
            return base.OnReceived(request, connectionId, data);
        }
    }
}
