
using System;
using System.Web;

namespace Beast.Web
{
	public static class HttpRequestBaseExtensions
	{
		public static string Host(this HttpRequestBase request)
		{
			return request.Host(null);
		}

		public static string Host(this HttpRequestBase request, string virtualPath)
		{
			if (request.Url == null)
				return string.Empty;

			var uri = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port, virtualPath ?? string.Empty);//request.IsLocal ? string.Concat(request.Url.Host, ":", request.Url.Port) : request.Url.Host);

			return uri.ToString();
		}
	}
}
