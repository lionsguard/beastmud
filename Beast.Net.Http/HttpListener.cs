using System.Web;

namespace Beast.Net
{
	public class HttpListener : IHttpHandler
	{
		public const string KeyConnectionId = "connectionId";

		public void ProcessRequest(HttpContext context)
		{
			var connId = context.Request.Params[KeyConnectionId];
			IConnection conn;
			if (string.IsNullOrEmpty(connId))
				conn = ConnectionManager.Create(new HttpConnectionFactory<StandardHttpConnection>(new JsonMessageFormatter()));
			else
				conn = ConnectionManager.Find(connId);

			if (conn == null)
			{
				// TODO: Error
				return;
			}

			// Process input
			conn.EnqueueInput(JsonInput.FromRequestParams(context.Request.Params));

			// Process any messages queued for this connection.
			if (conn is StandardHttpConnection)
			{
				var messages = (conn as StandardHttpConnection).DequeueMessages();
				foreach (var message in messages)
				{
					context.Response.Write(message);
				}
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}
