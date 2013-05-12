using System.Threading.Tasks;
using Beast.IO;
using Beast.Net.Http;
using Microsoft.AspNet.SignalR;

namespace Beast.Hosting.Web
{
    public class SignalRConnection : HttpConnection
    {
        public override void Write(IOutput output)
        {
            base.Write(output);

            Task.Run(() =>
                {
                    var ctx = GlobalHost.ConnectionManager.GetConnectionContext<WebHostConnection>();
                    if (ctx == null)
                        return;

                    var msgs = Read();
                    foreach (var msg in msgs)
                    {
                        ctx.Connection.Send(Id, msg);
                    }
                });
        }
    }
}
