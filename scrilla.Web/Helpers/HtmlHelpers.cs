using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace scrilla.Web.Helpers
{
	public static class ImageActionLinkHelper
	{
		public static IHtmlString InputActionLink(this AjaxHelper ajaxHelper, ActionResult result, AjaxOptions ajaxOptions)
		{
			var builder = new TagBuilder("input");
			var link = ajaxHelper.ActionLink("[replaceme]", result, ajaxOptions);
			var mvcLink = new MvcHtmlString(link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
			return mvcLink;
		}

		public static IHtmlString InputActionLink(this AjaxHelper ajaxHelper, ActionResult result, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var builder = new TagBuilder("input");
			builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			var link = ajaxHelper.ActionLink("[replaceme]", result, ajaxOptions);
			var mvcLink = new MvcHtmlString(link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
			return mvcLink;
		}

		public static IHtmlString ImageActionLink(this AjaxHelper ajaxHelper, ActionResult result, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			var builder = new TagBuilder("input");
			builder.MergeAttributes(htmlAttributes);
			var link = ajaxHelper.ActionLink("[replaceme]", result, ajaxOptions, htmlAttributes);
			var mvcLink = new MvcHtmlString(link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
			return mvcLink;
		}
	}
}