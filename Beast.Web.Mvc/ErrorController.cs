using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Beast.Web
{
	public class ErrorController : BeastController
	{
		public ActionResult Index()
		{
			return View("Error", GetErrorInfo());
		}

		public ActionResult NotFound()
		{
			return View(GetErrorInfo());
		}

		public ActionResult AccessDenied()
		{
			return View(GetErrorInfo());
		}

		public static void HandleError(HttpContext context)
		{
			try
			{
				var exception = context.Server.GetLastError();
				var httpException = exception as HttpException;
				context.Response.Clear();
				context.Server.ClearError();
				var routeData = new RouteData();
				routeData.Values["controller"] = "error";
				routeData.Values["action"] = "index";
				context.Response.StatusCode = 500;
				if (httpException != null)
				{
					context.Response.StatusCode = httpException.GetHttpCode();
					switch (context.Response.StatusCode)
					{
						case 403:
							routeData.Values["action"] = "AccessDenied";
							break;
						case 404:
							routeData.Values["action"] = "NotFound";
							break;
					}
				}

				if (context.Response.StatusCode == 500)
				{
					exception = new Exception(WebResources.HttpErrorDefaultMessage, exception);
				}

				routeData.Values["exception"] = exception;

				IController errorsController = new ErrorController();
				var rc = new RequestContext(new HttpContextWrapper(context), routeData);
				errorsController.Execute(rc);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				var msg = string.Empty;
#if DEBUG
				msg = ex.ToString();
#endif
				context.Response.Write(string.Format("<html><head><title>Error</title></head><p>Internal Error</p><p>{0}</p></html>",msg));
			}
		}
	}
}
