using System.Web;

namespace Beast.Net
{
	public class HttpListener : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			var input = JsonInput.FromRequestParams(context.Request.Params);
			var connId = context.Request.Params[HttpConstants.ConnectionId];
			IConnection conn;
			if (string.IsNullOrEmpty(connId))
				conn = ConnectionManager.Create(HttpConnection.Factory);
			else
				conn = ConnectionManager.Find(connId);

			if (conn == null)
			{
				var formatter = new JsonMessageFormatter();
				context.Response.Write(formatter.FormatMessage(new ResponseMessage(input).Invalidate(CommonResources.LoginRequired)));
				context.Response.End();
				return;
			}

			if (conn is HttpConnection)
			{
				(conn as HttpConnection).ProcessInput(input, context.Response);
			}
			else
			{
				// Process input for an IConnection instance.
				conn.EnqueueInput(input);	
			}
			context.Response.End();
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
