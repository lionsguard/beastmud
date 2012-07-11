
using System.Web;

namespace Beast.Web
{
	public static class HttpRequestBaseExtensions
	{
		public static string Host(this HttpRequestBase request)
		{
			if (request.Url == null)
				return string.Empty;

			return request.IsLocal ? string.Concat(request.Url.Host, ":", request.Url.Port) : request.Url.Host;
		}
	}
}
