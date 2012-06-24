using System.Web.Mvc;

namespace Beast.Web.Areas.Editor.Controllers
{
	public abstract class EditorController : BeastController
	{
		protected ActionResult RedirectToDashboard()
		{
			return RedirectToAction("index", "dashboard");
		}
	}
}