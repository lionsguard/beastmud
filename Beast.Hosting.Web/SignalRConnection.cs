using Beast.IO;
using Microsoft.AspNet.SignalR;

namespace Beast.Hosting.Web
{
    public class SignalRConnection : ConnectionBase
    {
        public override void Write(IOutput output)
        {
            var ctx = GlobalHost.ConnectionManager.GetConnectionContext<WebHostConnection>();
            if (ctx == null)
                return;

            ctx.Connection.Send(Id, output);
        }
    }
}
