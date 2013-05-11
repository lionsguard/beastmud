using Beast.IO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Web
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
