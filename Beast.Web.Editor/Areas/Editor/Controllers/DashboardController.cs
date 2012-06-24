using System.Web.Mvc;

namespace Beast.Web.Areas.Editor.Controllers
{
	public class DashboardController : EditorController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
