using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Beast.Web.Areas.Editor
{
	public class EditorAreaRegistration : PortableAreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Editor";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
			context.MapRoute(
				"Editor_default",
				"Editor/{controller}/{action}/{id}",
				new { controller="dashboard", action = "index", id = UrlParameter.Optional }
			);

			RegisterAreaEmbeddedResources();

			RegisterDefaultRoutes(context);
		}
	}
}
