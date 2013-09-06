using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(scrilla.js.Web.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace scrilla.js.Web.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/bootstrap/base") in the <head/> of your _Layout.cshtml view
			// For Bootstrap theme add @Styles.Render("~/Content/bootstrap/theme") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
			BundleTable.Bundles.Add(new ScriptBundle(Links.Bundles.Scripts.bootstrap, "//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min.js") { CdnFallbackExpression = "$.fn.button" }.Include(Links.scripts.bootstrap_js));
			//BundleTable.Bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrap, "//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css") { CdnFallbackExpression = "function(url){var length=document.styleSheets.length;for(var i = 0; i < length; i++){var sheet = document.styleSheets[i];if(sheet.href=url){var rules= sheet.rules ? sheet.rules : sheet.cssRules;if(rules==null || rules.length == 0){return false;}}}}('http://netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css')" }.Include(Links.Content.bootstrap.bootstrap_css));
			BundleTable.Bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrap).Include(Links.Content.bootstrap.bootstrap_css));
			BundleTable.Bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrapTheme).Include(Links.Content.bootstrap.bootstrap_theme_css));
		}
	}
}
