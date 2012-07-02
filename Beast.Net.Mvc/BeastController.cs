using System.Web.Mvc;
using System.Web.Security;

namespace Beast.Net
{
	public class BeastController : Controller
	{
		private IConnection _connection;
		protected IConnection Connection
		{
			get
			{
				if (_connection == null)
				{
					var connId = Request.Params[HttpConstants.ConnectionId];
					if (!string.IsNullOrEmpty(connId))
						_connection = ConnectionManager.Find(connId);
					else
						_connection = ConnectionManager.Create(HttpConnection.Factory);
				}
				return _connection;
			}
		}

		protected ResponseMessage ProcessInput(IInput input)
		{
			if (Connection == null)
				return null;

			var conn = MvcConnection.Wrap(Connection);
			return conn.ProcessInput(input);
		}

		protected void Authorize(string userId, bool persist)
		{
			FormsAuthentication.SetAuthCookie(userId, persist);
		}

		protected void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}