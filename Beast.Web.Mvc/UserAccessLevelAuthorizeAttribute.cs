using System.Web.Mvc;
using Beast.Security;

namespace Beast.Web
{
	public class UserAccessLevelAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly UserAccessLevel _level;

		public UserAccessLevelAuthorizeAttribute(UserAccessLevel level)
		{
			_level = level;
		}

		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			if (httpContext != null && httpContext.User.Identity.IsAuthenticated)
			{
				var user = Game.Current.Repository.GetUserById(httpContext.User.Identity.Name);
				if (user != null)
				{
					return (int) user.AccessLevel >= (int) _level;
				}
			}
			return false;
		}
	}
}
