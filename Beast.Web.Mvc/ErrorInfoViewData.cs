using System;
using System.Web;

namespace Beast.Web
{
	public class ErrorInfoViewData : System.Web.Mvc.HandleErrorInfo
	{
		public string Title { get; set; }
		public string ViewName { get; set; }
		public int StatusCode { get; set; }
		public bool DisplayErrorDetails { get; set; }

		public ErrorInfoViewData(Exception exception, string controllerName, string actionName)
			: base(exception, controllerName, actionName)
		{
			StatusCode = 500;
			if (exception is HttpException)
			{
				StatusCode = (exception as HttpException).GetHttpCode();
			}

			switch (StatusCode)
			{
				case 403:
					Title = WebResources.HttpError403Title;
					ViewName = "AccessDenied";
					break;
				case 404:
					Title = WebResources.HttpError404Title;
					ViewName = "NotFound";
					break;
				default:
					Title = WebResources.HttpError500Title;
					ViewName = "Error";
					break;
			}

#if DEBUG
			DisplayErrorDetails = true;
#endif
		}
	}
}
