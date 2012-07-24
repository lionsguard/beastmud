using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Beast.Web
{
	public static class LinkExtensions
	{
		public static MvcHtmlString MenuLink(
			this HtmlHelper htmlHelper,
			string linkText,
			string actionName,
			string controllerName,
			string currentCssClass
			)
		{
			var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
			var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			if (actionName.ToLower() == currentAction.ToLower() && controllerName.ToLower() == currentController.ToLower())
			{
				return htmlHelper.ActionLink(
					linkText,
					actionName,
					controllerName,
					null,
					new
					{
						@class = currentCssClass
					});
			}
			return htmlHelper.ActionLink(linkText, actionName, controllerName);
		}

		public static MvcHtmlString MenuListItem(
			this HtmlHelper htmlHelper,
			string linkText,
			string actionName,
			string controllerName,
			string currentCssClass
			)
		{
			return htmlHelper.MenuListItem(linkText, actionName, controllerName, currentCssClass, null);
		}

		public static MvcHtmlString MenuListItem(
			this HtmlHelper htmlHelper,
			string linkText,
			string actionName,
			string controllerName,
			string currentCssClass,
			IEnumerable<LinkData> subItems
			)
		{
			var link = htmlHelper.ActionLink(linkText, actionName, controllerName);
			var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
			var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
			var includeClass = (actionName.ToLower() == currentAction.ToLower() && controllerName.ToLower() == currentController.ToLower());

			var builder = new TagBuilder("li");
			if (includeClass)
				builder.AddCssClass(currentCssClass);

			builder.InnerHtml = link.ToHtmlString();

			if (subItems != null)
			{
				var ul = new TagBuilder("ul");
				foreach (var item in subItems)
				{
					var li = new TagBuilder("li");
					li.InnerHtml = htmlHelper.ActionLink(item.LinkText, item.Action, item.Controller).ToHtmlString();
					ul.InnerHtml += li;
				}
				builder.InnerHtml += ul;
			}

			return new MvcHtmlString(builder.ToString());
		}
	}

	public class LinkData
	{
		public string LinkText { get; set; }
		public string Action { get; set; }
		public string Controller { get; set; }
	}
}
