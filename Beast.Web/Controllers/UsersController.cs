using System.Web.Mvc;
using Beast.Net;
using Beast.Security;

namespace Beast.Web.Controllers
{
	[UserAccessLevelAuthorize(UserAccessLevel.God)]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
