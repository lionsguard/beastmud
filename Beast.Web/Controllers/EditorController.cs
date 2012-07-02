using System.Web.Mvc;
using Beast.Net;
using Beast.Security;

namespace Beast.Web.Controllers
{
	[UserAccessLevelAuthorize(UserAccessLevel.Builder)]
    public class EditorController : BeastController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
