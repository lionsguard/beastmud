using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Beast.Net;

namespace Beast.Web
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
					// Check user identity
					if (User != null && User.Identity.IsAuthenticated)
					{
						_connection = ConnectionManager.FindByUser(User.Identity.Name);
					}

					if (_connection == null)
					{
						var connId = string.Empty;
						if (string.IsNullOrEmpty(connId))
						{
							// Check request
							connId = Request.Params[HttpConstants.ConnectionId];
						}

						if (string.IsNullOrEmpty(connId))
						{
							// Check the session
							connId = (string) Session[HttpConstants.ConnectionId];
						}

						if (!string.IsNullOrEmpty(connId))
							_connection = ConnectionManager.Find(connId);
						else
							_connection = ConnectionManager.Create(HttpConnection.Factory);
					}
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

		protected ErrorInfoViewData GetErrorInfo()
		{
			return GetErrorInfo(GetRouteValue("exception", new Exception(WebResources.HttpErrorDefaultMessage)), ControllerContext.HttpContext);
		}

		protected ErrorInfoViewData GetErrorInfo(Exception ex, HttpContextBase context)
		{
			var model = new ErrorInfoViewData(ex, "error", "index");
			WriteLog(model.Exception, context.Request);

#if DEBUG
			model.DisplayErrorDetails = true;
#endif
			return model;
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			var info = GetErrorInfo(filterContext.Exception, filterContext.HttpContext);

			var result = new ViewResult();
			result.ViewName = info.ViewName;
			result.ViewData = new ViewDataDictionary<ErrorInfoViewData>(info);
			result.TempData = filterContext.Controller.TempData;

			filterContext.Result = result;
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = info.StatusCode;
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}
		
		protected T GetRouteValue<T>(string key, T defaultValue)
		{
			return RouteData.Values.Value<T>(key, defaultValue);
		}

		protected internal void WriteLog(Exception exception, HttpRequestBase request)
		{
			WriteLog(exception.ToString(), request);
		}

		protected internal void WriteLog(string exception, HttpRequestBase request)
		{
			var sb = new StringBuilder();
			sb.AppendLine(DateTime.Now.ToString());
			sb.AppendLine();
			sb.AppendLine(exception);
			sb.AppendLine();
			sb.AppendLine("FORM VALUES:");
			foreach (var item in request.Form.AllKeys)
			{
				sb.AppendFormat("{0} = {1}", item, request.Form[item]).AppendLine();
			}
			sb.AppendLine("QUERYSTRING VALUES:");
			foreach (var item in request.QueryString.AllKeys)
			{
				sb.AppendFormat("{0} = {1}", item, request.QueryString[item]).AppendLine();
			}
			sb.AppendLine();

#if DEBUG
			Log.Debug(sb.ToString());
#else
			Log.Error(sb.ToString());
#endif
		}
	}
}